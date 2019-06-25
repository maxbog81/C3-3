using System;
using System.Windows;
using System.Windows.Controls;

namespace MailSender.Components
{
    public partial class TabController
    {
        public event EventHandler LeftButtonClick;

        public event EventHandler RightButtonClick;

        public bool IsLeftButtonVisible
        {
            get => MoveLeft.Visibility == Visibility.Visible;
            set => MoveLeft.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool IsRightButtonVisible
        {
            get => MoveRight.Visibility == Visibility.Visible;
            set => MoveRight.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        public TabController() => InitializeComponent();

        public void OnButtonClick(object Sender, RoutedEventArgs E)
        {
            if (!(Sender is Button button)) return;

            switch (button.Name)
            {
                case "MoveLeft":
                    LeftButtonClick?.Invoke(this, EventArgs.Empty);
                    break;
                case "MoveRight":
                    RightButtonClick?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }
}
