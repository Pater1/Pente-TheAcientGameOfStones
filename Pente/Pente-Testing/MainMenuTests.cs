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
using Pente.Logic;

namespace Pente_Testing {
    [TestClass]
    public class MainMenuTests {
        [TestMethod]
        public void TestExit() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestExit_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestExit_TestLogic() {
            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;
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
                    AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
                });

                Thread.Sleep(10000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(_Window);
        }

        [TestMethod]
        public void TestGameStart_WithPlayerNames_WithDefaultSize() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithPlayerNames_WithDefaultSize_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithPlayerNames_WithDefaultSize_TestLogic() {
            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;
            bool works = true;
            App _App = null;
            try {
                Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

                _App = new App();
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
            } catch {
                works = false;
            } finally {
                _App?.Shutdown();
            }
            AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
        }

        [TestMethod]
        public void TestGameStart_WithoutPlayerNames() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithoutPlayerNames",
                AppDomain.CurrentDomain.Evidence,
                AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithoutPlayerNames_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithoutPlayerNames_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            Window _Window = new MainWindow();

            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;

            Task.Run(() => {
                Thread.Sleep(5000);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
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

                    bool failed = false;
                    try {
                        UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                    } catch {
                        failed = true;
                    }

                    AppDomain.CurrentDomain.SetData("AssertIsTrue", failed);
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
        
        [TestMethod]
        public void TestGameStart_WithoutPlayer1Name() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithoutPlayerNames",
                AppDomain.CurrentDomain.Evidence,
                AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithoutPlayer1Name_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithoutPlayer1Name_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            Window _Window = new MainWindow();

            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;

            Task.Run(() => {
                Thread.Sleep(5000);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
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

                    bool failed = false;
                    try {
                        UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                    } catch {
                        failed = true;
                    }
                    AppDomain.CurrentDomain.SetData("AssertIsTrue", failed);
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

        [TestMethod]
        public void TestGameStart_WithoutPlayer2Name() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithoutPlayerNames",
                AppDomain.CurrentDomain.Evidence,
                AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithoutPlayer2Name_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithoutPlayer2Name_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            Window _Window = new MainWindow();

            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;

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

                    bool failed = false;
                    try {
                        UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                    } catch {
                        failed = true;
                    }
                    AppDomain.CurrentDomain.SetData("AssertIsTrue", failed);
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
