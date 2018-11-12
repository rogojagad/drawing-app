using DrawingApp.Commands;
using DrawingApp.MenuItems;
using DrawingApp.ToolbarItems;
using DrawingApp.Tools;
using System.Diagnostics;
using System.Windows.Forms;

namespace DrawingApp
{
    public partial class MainWindow : Form
    {
        private IToolbox toolbox;
        private ICanvas canvas;
        private IToolbar toolbar;
        private IMenubar menubar;

        public MainWindow()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            Debug.WriteLine("Initializing UI objects.");

            #region Canvas
            Debug.WriteLine("Loading canvas...");
            this.canvas = new DefaultCanvas();
            this.toolStripContainer1.ContentPanel.Controls.Add((Control)this.canvas);
            #endregion

            #region Commands

            BlackCanvasBgCommands blackCanvasBgCommands = new BlackCanvasBgCommands(this.canvas);
            WhiteCanvasBgCommands whiteCanvasBgCommands = new WhiteCanvasBgCommands(this.canvas);

            #endregion

            #region Menubar
            Debug.WriteLine("Loading menubar...");
            this.menubar = new DefaultMenubar();
            this.Controls.Add((Control)this.menubar);

            DefaultMenuItem fileMenuItem = new DefaultMenuItem("File");
            this.menubar.AddMenuItem(fileMenuItem);

            DefaultMenuItem newMenuItem = new DefaultMenuItem("New");
            fileMenuItem.AddMenuItem(newMenuItem);

            DefaultMenuItem editMenuItem = new DefaultMenuItem("Edit");
            this.menubar.AddMenuItem(editMenuItem);

            DefaultMenuItem changeToBlackMenuItem = new DefaultMenuItem("Change to Black");
            changeToBlackMenuItem.SetCommand(blackCanvasBgCommands);
            editMenuItem.AddMenuItem(changeToBlackMenuItem);

            DefaultMenuItem changeToWhiteMenuItem = new DefaultMenuItem("Change to White");
            changeToWhiteMenuItem.SetCommand(whiteCanvasBgCommands);
            editMenuItem.AddMenuItem(changeToWhiteMenuItem);

            #endregion

            #region Toolbox

            // Initializing toolbox
            Debug.WriteLine("Loading toolbox...");
            this.toolbox = new DefaultToolbox();
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add((Control)this.toolbox);

            #endregion

            #region Tools

            // Initializing tools
            Debug.WriteLine("Loading tools...");
            this.toolbox.AddTool(new SelectionTool());
            this.toolbox.AddSeparator();
            this.toolbox.AddTool(new LineTool());
            this.toolbox.AddTool(new RectangleTool());
            this.toolbox.AddTool(new TextTool());
            this.toolbox.ToolSelected += Toolbox_ToolSelected;

            #endregion

            #region Toolbar
            // Initializing toolbar
            Debug.WriteLine("Loading toolbar...");
            this.toolbar = new DefaultToolbar();
            this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control)this.toolbar);

            ExampleToolbarItem whiteBackgroundTool = new ExampleToolbarItem();
            whiteBackgroundTool.SetCommand(whiteCanvasBgCommands);
            ExampleToolbarItem blackBackgroundTool = new ExampleToolbarItem();
            blackBackgroundTool.SetCommand(blackCanvasBgCommands);

            this.toolbar.AddToolbarItem(whiteBackgroundTool);
            this.toolbar.AddSeparator();
            this.toolbar.AddToolbarItem(blackBackgroundTool);
            #endregion

        }

        private void Toolbox_ToolSelected(ITool tool)
        {
            if (this.canvas != null)
            {
                Debug.WriteLine("Tool " + tool.Name + " is selected");
                this.canvas.SetActiveTool(tool);
                tool.TargetCanvas = this.canvas;
            }
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, System.EventArgs e)
        {

        }
    }
}
