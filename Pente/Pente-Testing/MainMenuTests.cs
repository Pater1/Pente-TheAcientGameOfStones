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
    public class MainMenuTests {
        [TestMethod]
        public void TestExit() {
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
                            return x.Name == "Exit";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "Exit_Click", b);
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    bool works = false;
                    try {
                        _App.MainWindow.Focus();
                    } catch {
                        works = true;
                    }
                    Assert.IsTrue(works);
                });

                Thread.Sleep(10000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(_Window);
        }
        [TestMethod]
        public void TestGameStart_basic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            Window _Window = new MainWindow();

            Task.Run(() => {
                Thread.Sleep(5000);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(1000);

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

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
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
