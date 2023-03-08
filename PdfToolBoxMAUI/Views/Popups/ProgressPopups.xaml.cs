using CommunityToolkit.Maui.Views;

namespace PdfToolBoxMAUI.Views.Popups;

public partial class ProgressPopups : Popup
{
	public ProgressPopups()
	{
		InitializeComponent();
	}

	public string ProgressText { get { return lblProgress.Text; } set { lblProgress.Text = value; } }

	private void Button_Clicked(object sender, EventArgs e)
	{
		this.Close();
    }
}