using battleships.Utils;
using Godot;

namespace battleships.UI;

public partial class PartButton : Button
{
	[Export] public Globals.Parts Part { get; set; } = Globals.Parts.None;

	public PartButton()
	{
		CustomMinimumSize = new Vector2(136, 136);
		Pressed += SelectPart;
	}

	private void SelectPart()
	{
		Globals.SelectedPart = Part;
	}

}