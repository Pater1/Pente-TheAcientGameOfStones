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
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    bool works = false;
                    try {
                        _App.MainWindow.Focus();
                    } catch {
                        works = true;
                    }
                    AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
                });

                Thread.Sleep(1000);
            
                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
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
                

                Task.Run(() => {
                    Thread.Sleep(500);

                    MainDispatcher.Invoke(() => {
                        TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player1Name";
                            }
                        ));
                        t1.Text = "P1";
                        TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player2Name";
                            }
                        ));
                        t2.Text = "P2";
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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
                    MainDispatcher.Invoke(() => {
                        _App.MainWindow.Focus();
                    });

                    Thread.Sleep(1000);

                    MainDispatcher.Invoke(() => {
                        _App.Shutdown();
                    });
                });
                _App.Run(new MainWindow());
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
            

            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
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
            

            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
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
            

            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }

        [TestMethod]
        public void TestGameStart_WithPlayerNames_WithMinSize() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithPlayerNames_WithMinSize_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithPlayerNames_WithMinSize_TestLogic() {
            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;
            bool works = true;
            App _App = null;
            try {
                Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

                _App = new App();
                _App.InitializeComponent();
                

                Task.Run(() => {
                    Thread.Sleep(500);

                    MainDispatcher.Invoke(() => {
                        TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player1Name";
                            }
                        ));
                        t1.Text = "P1";
                        TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player2Name";
                            }
                        ));
                        t2.Text = "P2";

                        TextBox gx = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridXSize";
                            }
                        ));
                        gx.Text = "9";
                        TextBox gy = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridYSize";
                            }
                        ));
                        gy.Text = "9";
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.GetType() == typeof(GameScreen);
                            }
                        ));
                        UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GameGrid";
                            }
                        ));
                        int rows = ug.Rows;
                        int columns = ug.Columns;

                        AppDomain.CurrentDomain.SetData("AssertTrue", rows == 9 && columns == 9);
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        _App.MainWindow.Focus();
                    });

                    Thread.Sleep(1000);

                    MainDispatcher.Invoke(() => {
                        _App.Shutdown();
                    });
                });
                _App.Run(new MainWindow());
            } catch {
                works = false;
            } finally {
                _App?.Shutdown();
            }
            AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
        }
        [TestMethod]
        public void TestGameStart_WithPlayerNames_WithMaxSize() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithPlayerNames_WithMaxSize_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithPlayerNames_WithMaxSize_TestLogic() {
            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;
            bool works = true;
            App _App = null;
            try {
                Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

                _App = new App();
                _App.InitializeComponent();
                

                Task.Run(() => {
                    Thread.Sleep(500);

                    MainDispatcher.Invoke(() => {
                        TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player1Name";
                            }
                        ));
                        t1.Text = "P1";
                        TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player2Name";
                            }
                        ));
                        t2.Text = "P2";

                        TextBox gx = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridXSize";
                            }
                        ));
                        gx.Text = "39";
                        TextBox gy = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridYSize";
                            }
                        ));
                        gy.Text = "39";
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.GetType() == typeof(GameScreen);
                            }
                        ));
                        UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GameGrid";
                            }
                        ));
                        int rows = ug.Rows;
                        int columns = ug.Columns;

                        AppDomain.CurrentDomain.SetData("AssertTrue", rows == 39 && columns == 39);
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        _App.MainWindow.Focus();
                    });

                    Thread.Sleep(1000);

                    MainDispatcher.Invoke(() => {
                        _App.Shutdown();
                    });
                });
                _App.Run(new MainWindow());
            } catch {
                works = false;
            } finally {
                _App?.Shutdown();
            }
            AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
        }
        [TestMethod]
        public void TestGameStart_WithPlayerNames_WithMinXMaxYSize() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithPlayerNames_WithMinXMaxYSize_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithPlayerNames_WithMinXMaxYSize_TestLogic() {
            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;
            bool works = true;
            App _App = null;
            try {
                Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

                _App = new App();
                _App.InitializeComponent();
                

                Task.Run(() => {
                    Thread.Sleep(500);

                    MainDispatcher.Invoke(() => {
                        TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player1Name";
                            }
                        ));
                        t1.Text = "P1";
                        TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player2Name";
                            }
                        ));
                        t2.Text = "P2";

                        TextBox gx = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridXSize";
                            }
                        ));
                        gx.Text = "9";
                        TextBox gy = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridYSize";
                            }
                        ));
                        gy.Text = "39";
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.GetType() == typeof(GameScreen);
                            }
                        ));
                        UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GameGrid";
                            }
                        ));
                        int rows = ug.Rows;
                        int columns = ug.Columns;

                        AppDomain.CurrentDomain.SetData("AssertTrue", rows == 9 && columns == 39);
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        _App.MainWindow.Focus();
                    });

                    Thread.Sleep(1000);

                    MainDispatcher.Invoke(() => {
                        _App.Shutdown();
                    });
                });
                _App.Run(new MainWindow());
            } catch {
                works = false;
            } finally {
                _App?.Shutdown();
            }
            AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
        }
        [TestMethod]
        public void TestGameStart_WithPlayerNames_WithMinYMaxXSize() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameStart_WithPlayerNames_WithMinYMaxXSize_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestGameStart_WithPlayerNames_WithMinYMaxXSize_TestLogic() {
            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;
            bool works = true;
            App _App = null;
            try {
                Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

                _App = new App();
                _App.InitializeComponent();
                

                Task.Run(() => {
                    Thread.Sleep(500);

                    MainDispatcher.Invoke(() => {
                        TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player1Name";
                            }
                        ));
                        t1.Text = "P1";
                        TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player2Name";
                            }
                        ));
                        t2.Text = "P2";

                        TextBox gx = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridXSize";
                            }
                        ));
                        gx.Text = "39";
                        TextBox gy = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GridYSize";
                            }
                        ));
                        gy.Text = "9";
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.GetType() == typeof(GameScreen);
                            }
                        ));
                        UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "GameGrid";
                            }
                        ));
                        int rows = ug.Rows;
                        int columns = ug.Columns;

                        AppDomain.CurrentDomain.SetData("AssertTrue", rows == 39 && columns == 9);
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        _App.MainWindow.Focus();
                    });

                    Thread.Sleep(1000);

                    MainDispatcher.Invoke(() => {
                        _App.Shutdown();
                    });
                });
                _App.Run(new MainWindow());
            } catch {
                works = false;
            } finally {
                _App?.Shutdown();
            }
            AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
        }

        [TestMethod]
        public void TestPlayerNamesPersistOnGameStart() {
            AppDomain dmn = AppDomain.CreateDomain("TestPlayerNamesPersistOnGameStart",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestPlayerNamesPersistOnGameStart_TestLogic);

            Assert.IsTrue((bool)dmn.GetData("AssertIsTrue"));
        }
        private static void TestPlayerNamesPersistOnGameStart_TestLogic() {
            MainGameLogic.ThrowErrorInsteadOfMessageBox = true;
            bool works = true;
            App _App = null;
            try {
                Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

                _App = new App();
                _App.InitializeComponent();


                Task.Run(() => {
                    Thread.Sleep(500);

                    MainDispatcher.Invoke(() => {
                        TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player1Name";
                            }
                        ));
                        t1.Text = "P1";
                        TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player2Name";
                            }
                        ));
                        t2.Text = "P2";
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
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

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.GetType() == typeof(GameScreen);
                            }
                        ));

                        PlayerStatsUC p1 = UIAutomationHelper.FindVisualChild<PlayerStatsUC>(gs, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player1Status";
                            }
                        ));
                        PlayerStatsUC p2 = UIAutomationHelper.FindVisualChild<PlayerStatsUC>(gs, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "Player2Status";
                            }
                        ));

                        Label p1l = UIAutomationHelper.FindVisualChild<Label>(p1, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "MainLabel";
                            }
                        ));
                        Label p2l = UIAutomationHelper.FindVisualChild<Label>(p2, (Func<FrameworkElement, bool>)(
                            (x) => {
                                return x.Name == "MainLabel";
                            }
                        ));

                        bool shouldBeTrue =
                            (p1l.Content as string).Contains("P1") &&
                            (p2l.Content as string).Contains("P2");
                        AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                    });

                    Thread.Sleep(100);

                    MainDispatcher.Invoke(() => {
                        _App.MainWindow.Focus();
                    });

                    Thread.Sleep(1000);

                    MainDispatcher.Invoke(() => {
                        _App.Shutdown();
                    });
                });
                _App.Run(new MainWindow());
            } catch {
                works = false;
            } finally {
                _App?.Shutdown();
            }
            AppDomain.CurrentDomain.SetData("AssertIsTrue", works);
        }

    }
}
