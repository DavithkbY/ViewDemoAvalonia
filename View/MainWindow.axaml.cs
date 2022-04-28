using System;
using System.Windows.Input;
using Avalonia.Controls;
using Cells;

namespace View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    /*
     *  View model classes should of course be placed in a separate project.
     */
    public class MainViewModel
    {
        public MainViewModel()
        {
            // Create empty cell
            CurrentScreen = Cell.Create<ScreenViewModel>(null);

            // Create screen A
            var firstScreen = new ScreenAViewModel(CurrentScreen);

            // Put first screen in CurrentScreen cell
            CurrentScreen.Value = firstScreen;
        }

        public ICell<ScreenViewModel> CurrentScreen { get; }
    }

    /// <summary>
    /// Superclass for each screen
    /// </summary>
    public abstract class ScreenViewModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="currentScreen">
        /// Cell containing the current screen
        /// </param>
        protected ScreenViewModel(ICell<ScreenViewModel> currentScreen)
        {
            this.CurrentScreen = currentScreen;
        }

        /// <summary>
        /// Cell containing the current screen.
        /// Overwrite the contents of this cell to switch screens.
        /// </summary>
        protected ICell<ScreenViewModel> CurrentScreen { get; }
    }

    public class ScreenAViewModel : ScreenViewModel
    {
        public ScreenAViewModel(ICell<ScreenViewModel> currentScreen) : base(currentScreen)
        {
            // SwitchToScreenB command, when activated, will create a ScreenBViewModel and put it in the CurrentScreen cell
            SwitchToScreenB = new ActionCommand(() => CurrentScreen.Value = new ScreenBViewModel(this.CurrentScreen));

            // SwitchToScreenC command, when activated, will create a ScreenCViewModel and put it in the CurrentScreen cell
            SwitchToScreenC = new ActionCommand(() => CurrentScreen.Value = new ScreenCViewModel(this.CurrentScreen));
        }

        public ICommand SwitchToScreenB { get; }

        public ICommand SwitchToScreenC { get; }
    }

    public class ScreenBViewModel : ScreenViewModel
    {
        public ScreenBViewModel(ICell<ScreenViewModel> currentScreen) : base(currentScreen)
        {
            SwitchToScreenA = new ActionCommand(() => CurrentScreen.Value = new ScreenAViewModel(this.CurrentScreen));
            SwitchToScreenC = new ActionCommand(() => CurrentScreen.Value = new ScreenCViewModel(this.CurrentScreen));
        }

        public ICommand SwitchToScreenA { get; }

        public ICommand SwitchToScreenC { get; }
    }

    public class ScreenCViewModel : ScreenViewModel
    {
        public ScreenCViewModel(ICell<ScreenViewModel> currentScreen) : base(currentScreen)
        {
            SwitchToScreenA = new ActionCommand(() => CurrentScreen.Value = new ScreenAViewModel(this.CurrentScreen));
            SwitchToScreenB = new ActionCommand(() => CurrentScreen.Value = new ScreenBViewModel(this.CurrentScreen));
        }

        public ICommand SwitchToScreenA { get; }

        public ICommand SwitchToScreenB { get; }
    }

    public class ActionCommand : ICommand
    {
        private readonly Action action;

        public ActionCommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action();
        }
    }
        
    
}