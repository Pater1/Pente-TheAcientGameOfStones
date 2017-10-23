using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

using System.Threading;
using System.Threading.Tasks;

using Pente;
using Pente.UserControls;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pente_Testing {
    [TestClass]
    public class GameplayTests {
        [TestMethod]
        public void TestOnlyMiddleStoneOnFirstMove() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            Window _Window = new MainWindow();
            
            Task.Run(() => {
                Thread.Sleep(5000);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));
                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    b.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(gs, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    UIElementCollection uiec = ug.Children;
                    foreach(UIElement uie in uiec) {
                        int index = ug.Children.IndexOf(uie);

                        int row = index / columns;
                        int column = index % columns;

                        int centerRow = rows / 2;
                        int centerColumn = columns / 2;

                        int distToCenterRow = Math.Abs(row - centerRow);
                        int distToCenterColumn = Math.Abs(column - centerColumn);

                        if(distToCenterColumn == 0 && distToCenterRow == 0) {
                            UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", uie);
                        }
                    }
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(10000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(_Window);
        }
    }
}
