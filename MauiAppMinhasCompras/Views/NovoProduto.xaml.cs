using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    private readonly List<string> categorias = new List<string> { "Alimentos", "Higiene", "Limpeza" };
    public NovoProduto()
	{
		InitializeComponent();
        pickerCategoria.ItemsSource = categorias;
        pickerCategoria.SelectedIndex = 0;
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert("Ops", "Por favor, preencha a descrição", "OK");
                return;
            }

            if (!double.TryParse(txt_quantidade.Text, out double quantidade) || quantidade <= 0)
            {
                await DisplayAlert("Ops", "Quantidade deve ser maior que zero e numérica", "OK");
                return;
            }

            if (!double.TryParse(txt_preco.Text, out double preco) || preco <= 0)
            {
                await DisplayAlert("Ops", "Preço deve ser maior que zero e numérico", "OK");
                return;
            }

            if (pickerCategoria.SelectedIndex < 0)
            {
                await DisplayAlert("Ops", "Por favor, selecione uma categoria", "OK");
                return;
            }

            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = quantidade,
                Preco = preco,
                Categoria = pickerCategoria.SelectedItem.ToString()
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucesso!", "Registro Inserido", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }



}