using System.Windows.Input;

namespace PdfToolBoxMAUI.Views.Controls;

public partial class PdfCollectionView : ContentView
{


	private ObservableCollection<PdfItemUI> _items;
	public ObservableCollection<PdfItemUI> Items {
		get
		{
			if(_items == null)
			{
				_items = new();
				for (int i = 0; i < 100; i++)
				{
					_items.Add(new PdfItemUI(_items, new PdfCollectionViewItem() { Id = i, Name = $"item {i}" }));
				}
			}
			return _items;
		}
	}


	public PdfCollectionView()
	{
		InitializeComponent();



		//BindableLayout.SetItemsSource(flx, Items);
	}









	private void Button_Clicked(object sender, EventArgs e)
	{
		BindableLayout.SetItemsSource(flx, Items);
	}
}

public class PdfItemUI
{
	public ObservableCollection<PdfItemUI> ItemsCollection { get; private set; }
	public PdfCollectionViewItem Data { get; private set; }
	public ICommand DraggedCommand { get; set; }
	public ICommand DragCompletedCommand { get; set; }


	public ICommand DraggedOverCommandRight { get; }
	public ICommand DragLeaveCommandRight { get; }
	public ICommand DroppedCommandRight { get; }


	public ICommand DraggedOverCommandLeft { get; }
	public ICommand DragLeaveCommandLeft { get; }
	public ICommand DroppedCommandLeft { get; }

	public bool BeingDragged { get; set; }


	public static readonly Color DragOverUIColor = new Color(255, 0, 0, 255);
	public static readonly Color DragLeaveUIColor = new Color(0, 255, 0, 255);

	public Color DropRightUIColor { get; private set; } = DragLeaveUIColor;
	public Color DropLeftUIColor { get; private set; } = DragLeaveUIColor;
	public bool BoxViewVisibleLeft { get { return Data.Id == ItemsCollection[0].Data.Id; } }
	public bool BoxViewVisibleRight { get; private set; } = true;
	public PdfItemUI(ObservableCollection<PdfItemUI> coll, PdfCollectionViewItem data)
	{
		Data = data;
		ItemsCollection = coll;

		DraggedCommand = new Command<PdfItemUI>(DragStarting);
		DragCompletedCommand = new Command<PdfItemUI>(DragCompleted);


		DraggedOverCommandRight = new Command<PdfItemUI>(DragOverRight);
		DragLeaveCommandRight = new Command<PdfItemUI>(DragLeaveRight);
		DroppedCommandRight = new Command<PdfItemUI>(DropRight);

		DraggedOverCommandLeft = new Command<PdfItemUI>(DragOverLeft);
		DragLeaveCommandLeft = new Command<PdfItemUI>(DragLeaveLeft);
		DroppedCommandLeft = new Command<PdfItemUI>(DropLeft);
	}

	private void DragStarting(PdfItemUI param)
	{
		param.BeingDragged = true;
		param.DropLeftUIColor = DragOverUIColor;

	}
	private void DragCompleted(PdfItemUI param)
	{
		param.BeingDragged = false;

	}

	
	
	private void DropLeft(PdfItemUI param)
	{

		int drag_itm_idx = -1;
		int curr_idm_idx = -1;
		for (int i = 0; i < ItemsCollection.Count; i++)
		{
			if (ItemsCollection[i].BeingDragged)
			{
				drag_itm_idx = i;
			}
			if (ItemsCollection[i].Data.Id == param.Data.Id)
			{
				curr_idm_idx = i;
			}

			if (drag_itm_idx is not -1 && curr_idm_idx is not -1)
			{
				break;
			}
		}

		if (drag_itm_idx is not -1)
		{

			if (drag_itm_idx > curr_idm_idx)
			{
				var tmp = ItemsCollection[drag_itm_idx];

				for (int j = drag_itm_idx; j > curr_idm_idx; j--)
				{
					ItemsCollection.Move(j - 1, j);
				}
				ItemsCollection[curr_idm_idx] = tmp;
			}
			else if (drag_itm_idx < curr_idm_idx)
			{
				curr_idm_idx--;
				var tmp = ItemsCollection[drag_itm_idx];
				for (int j = drag_itm_idx; j < curr_idm_idx; j++)
				{
					ItemsCollection.Move(j + 1, j);
				}
				ItemsCollection[curr_idm_idx] = tmp;
			}
		}
	}
	private void DropRight(PdfItemUI param)
	{
		int drag_itm_idx = -1;
		int curr_idm_idx = -1;
		for (int i = 0; i < ItemsCollection.Count; i++)
		{
			if (ItemsCollection[i].BeingDragged)
			{
				drag_itm_idx = i;
			}
			if (ItemsCollection[i].Data.Id == param.Data.Id)
			{
				curr_idm_idx = i;
			}

			if (drag_itm_idx is not -1 && curr_idm_idx is not -1)
			{
				break;
			}
		}
		
		if (drag_itm_idx is not -1)
		{
			
			if (drag_itm_idx > curr_idm_idx)
			{
				curr_idm_idx++;
				var tmp = ItemsCollection[drag_itm_idx];

				for (int j = drag_itm_idx; j > curr_idm_idx; j--)
				{
					ItemsCollection.Move(j -1, j);
				}
				ItemsCollection[curr_idm_idx] = tmp;
			}
			else if (drag_itm_idx < curr_idm_idx)
			{
				var tmp = ItemsCollection[drag_itm_idx];
				for (int j = drag_itm_idx; j < curr_idm_idx; j++)
				{
					ItemsCollection.Move(j + 1, j);
				}
				ItemsCollection[curr_idm_idx] = tmp;
			}
		}
	}
	
	
	private void DragOverRight(PdfItemUI param)
	{
		if (param.DropRightUIColor != DragOverUIColor)
		{
			param.DropRightUIColor = DragOverUIColor;
		}
	}

	private void DragLeaveRight(PdfItemUI param)
	{
		if (param.DropRightUIColor != DragLeaveUIColor)
		{
			param.DropRightUIColor = DragLeaveUIColor;
		}
	}
	private void DragOverLeft(PdfItemUI param)
	{
		if (param.DropLeftUIColor != DragOverUIColor)
		{
			param.DropLeftUIColor = DragOverUIColor;
		}
	}

	private void DragLeaveLeft(PdfItemUI param)
	{
		if (param.DropLeftUIColor != DragLeaveUIColor)
		{
			param.DropLeftUIColor = DragLeaveUIColor;
		}
	}

}
