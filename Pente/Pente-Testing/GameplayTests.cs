using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

using System.Threading;
using System.Threading.Tasks;

using System.Reflection;

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
                    Button b = FindVisualChild<Button>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));
                    MainMenu mm = FindVisualChild<MainMenu>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                   b.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    UniformGrid ug = FindVisualChild<UniformGrid>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    GameScreen gs = FindVisualChild<GameScreen>(_Window, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
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
                            Click(gs, "StoneOnMouseLeftButtonDown", uie);
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

        public static T FindVisualChild<T>(FrameworkElement depObj, Func<FrameworkElement, bool> queary) where T : FrameworkElement {
            if (depObj != null) {
                T ret = depObj as T;
                if(ret != null) {
                    return ret;
                }

                Type t = depObj.GetType();
                PropertyInfo pfi = t.GetProperty("Children");
                if(pfi != null) {
                    object o = pfi.GetValue(depObj);
                    if (o != null) {
                        UIElementCollection uiec = o as UIElementCollection;
                        if (uiec != null) {
                            foreach (UIElement uie in uiec) {
                                FrameworkElement fr = uie as FrameworkElement;
                                if (fr != null) {
                                    ret = FindVisualChild<T>(fr, queary);
                                    if (ret != null && queary(ret)) {
                                        return ret;
                                    }
                                }
                            }
                        }
                    }
                }
                pfi = t.GetProperty("Child");
                if (pfi != null) {
                    object o = pfi.GetValue(depObj);
                    if (o != null) {
                        FrameworkElement uie = o as FrameworkElement;
                        if (uie != null) {
                            ret = FindVisualChild<T>(uie, queary);
                            if (ret != null && queary(ret)) {
                                return ret;
                            }
                        }
                    }
                }
                pfi = t.GetProperty("Content");
                if (pfi != null) {
                    object o = pfi.GetValue(depObj);
                    if (o != null) {
                        FrameworkElement uie = o as FrameworkElement;
                        if (uie != null) {
                            ret = FindVisualChild<T>(uie, queary);
                            if (ret != null && queary(ret)) {
                                return ret;
                            }
                        }
                    }
                } 
            }
            return null;
        }
        private void Click(object logicHolder, string funcReflect, object clicked) {
            Type t = logicHolder.GetType();
            MethodInfo meth = t.GetMethod(funcReflect, BindingFlags.NonPublic | BindingFlags.Instance);
            meth.Invoke(logicHolder, new object[] { clicked, null });
        }
    }
}
