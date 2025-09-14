using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;
public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    private readonly List<string> categorias = new List<string> { "Todos", "Alimentos", "Higiene", "Limpeza" };

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            pickerCategoria.ItemsSource = categorias;
            pickerCategoria.SelectedIndex = 0; // Seleciona "Todos" por padrão
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async Task CarregarProdutos(string categoria = "Todos", string busca = "")
    {
        lista.Clear();
        List<Produto> produtos;

        if (categoria == "Todos" && string.IsNullOrWhiteSpace(busca))
        {
            produtos = await App.Db.GetAll();
        }
        else if (categoria == "Todos")
        {
            produtos = await App.Db.Search(busca);
        }
        else if (string.IsNullOrWhiteSpace(busca))
        {
            produtos = await App.Db.GetByCategoria(categoria);
        }
        else
        {
            produtos = await App.Db.SearchByCategoria(busca, categoria);
        }
        produtos.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked(object sender, System.EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (System.Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        try
        {
            string textoBusca = e.NewTextValue;
            string categoriaSelecionada = pickerCategoria.SelectedItem?.ToString() ?? "Todos";
            await CarregarProdutos(categoriaSelecionada, textoBusca);
        }
        catch (System.Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, System.EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);
            string msg = $"O total é {soma:C}";
            DisplayAlert("Total dos Produtos", msg, "OK");
        }
        catch (System.Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, System.EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;
            Produto p = selecionado.BindingContext as Produto;
            bool confirm = await DisplayAlert(
                "Tem certeza?", $"Remover {p.Descricao}?", "Sim", "Nâo");
            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (System.Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_ItemSelected(object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;
            if (p != null)
            {
                await Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p,
                });
            }
        }
        catch (System.Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, System.EventArgs e)
    {
        try
        {
            string categoriaSelecionada = pickerCategoria.SelectedItem?.ToString() ?? "Todos";
            string textoBusca = txt_search.Text ?? "";
            await CarregarProdutos(categoriaSelecionada, textoBusca);
        }
        catch (System.Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void pickerCategoria_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        try
        {
            string categoriaSelecionada = pickerCategoria.SelectedItem?.ToString() ?? "Todos";
            string textoBusca = txt_search.Text ?? "";
            await CarregarProdutos(categoriaSelecionada, textoBusca);
        }
        catch (System.Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
