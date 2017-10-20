using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Test.Input;
//using System.Windows.Input;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Automation.Text;
//using FlaUI.UIA3;
//using FlaUI.Core.AutomationElements;
//using FlaUI.Core;
//using FlaUI.Core.AutomationElements.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Pente_Testing {
    [TestClass]
    public class GameplayTests {
        [TestMethod]
        public void TestOnlyMiddleStoneOnFirstMove() {
            Process p = Process.Start(TestResources.ApplicationPath);
            while (p.MainWindowHandle == IntPtr.Zero || p.MainWindowHandle == null) { }

            AutomationElement window = AutomationElement.FromHandle(p.MainWindowHandle);

            AutomationElement startGame = window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "NewGameButton"));

            System.Windows.Point pnt = startGame.GetClickablePoint();
            Mouse.MoveTo(new System.Drawing.Point((int)pnt.X, (int)pnt.Y));
            Mouse.Click(MouseButton.Left);
            
            AutomationElement gameScreen = window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, "GameScreen"));

            AutomationElement[][] grid = GetGridArray(window);
            for(int x = 0; x < grid.Length; x++) {
                for(int y = 0; y < grid[x].Length; y++) {
                    int hx = grid.Length / 2, hy = grid[x].Length / 2;
                    if(x!=hx&&y!=hy)Click(grid[x][y]);
                }
            }

            p.Kill();
        }
        private void Click(AutomationElement ae, MouseButton b = MouseButton.Left) {
            System.Windows.Point pnt = ae.GetClickablePoint();
            Mouse.MoveTo(new System.Drawing.Point((int)pnt.X, (int)pnt.Y));
            Mouse.Click(b);
        }
        private AutomationElement[][] GetGridArray(AutomationElement window) {
            List<List<AutomationElement>> tmp = new List<List<AutomationElement>>();
            while (true) {
                AutomationElement cell = window.FindFirst(
                    TreeScope.Descendants, 
                    new PropertyCondition(
                        AutomationElement.NameProperty, 
                        $"0,{tmp.Count}"));
                if(cell != null) {
                    tmp.Add(new List<AutomationElement>() {cell});
                } else {
                    break;
                }
            }

            for(int x = 0; x < tmp.Count; x++) {
                while (true) {
                    AutomationElement cell = window.FindFirst(
                        TreeScope.Descendants, 
                        new PropertyCondition(
                            AutomationElement.NameProperty, 
                            $"{tmp[x].Count},{x}"));
                    if (cell != null) {
                        tmp[x].Add(cell);
                    } else {
                        break;
                    }
                }
            }

            return tmp.Select(x => x.ToArray()).ToArray();
        }
        private string NameAllDecendants(AutomationElement ae) {
            string ret = "";
            foreach (AutomationElement a in ae.FindAll(TreeScope.Descendants, Condition.TrueCondition)) {
                ret += $"-{a.Current.Name}";
            }
            return ret;
        }
        private string NameAllDecendants(AutomationElement[][] ae) {
            string ret = "";
            foreach (AutomationElement[] a in ae) {
                foreach (AutomationElement e in a) {
                    ret += $"-{e.Current.Name}";
                }
            }
            return ret;
        }

        //private void StartGame(out Application app, out UIA3Automation automation, out Window window, out AutomationElement board) {
        //    app = Application.Launch(TestResources.ApplicationPath);

        //    automation = new UIA3Automation();

        //    window = app.GetMainWindow(automation);
        //    Button but = window.FindFirstDescendant(x => x.ByAutomationId("NewGameButton")).AsButton();

        //    but.Click();

        //    board = window.FindFirstDescendant(x => x.ByClassName("GameScreen"));

        //    //string tmp = "";
        //    //foreach(AutomationElement v in window.FindAllDescendants()) {
        //    //    tmp += $"-{v.Properties.Name}";
        //    //}
        //    //throw new ExecutionEngineException(tmp);
        //}
        //[TestMethod]
        //public void TestOnlyMiddleStoneOnFirstMove() {
        //    StartGame(out Application app, out UIA3Automation automation, out Window window, out AutomationElement board);
        //    var v = window.FindFirstDescendant(x => x.ByName("0,0"));
        //    if(v == null) {
        //        throw new ExecutionEngineException("v is null");
        //    }
        //    Grid gameBoard = v.AsGrid();
        //    //throw new ExecutionEngineException("Got to gameBoard");

        //    LoadGameDiffCache(gameBoard);
        //    if (DidGameDiff) {
        //        throw new ExecutionEngineException("Game diffed before changes");
        //    }
        //    int rows = gameBoard.RowCount;
        //    for(int r = 0; r < rows; r++) {
        //        GridRow boardRow = gameBoard.GetRowByIndex(r);
        //        int collumns = boardRow.Cells.Length;
        //        for(int c = 0; c < collumns; c++) {
        //            if(!(r == (rows/2+1) && c == (collumns / 2 + 1)) ) {
        //                GridCell cell = boardRow.Cells[c];
        //                cell.Click();
        //            }
        //        }
        //    }
        //    if (DidGameDiff) {
        //        throw new ExecutionEngineException("Game diffed after changes, before should");
        //    }
        //    if (DidGameDiff) {
        //        throw new ExecutionEngineException("Game diffed after changes, before should");
        //    }

        //    window.Close();
        //    automation.Dispose();
        //}

        //BitmapImage cachedState;
        //AutomationElement cachedElement;
        //private void LoadGameDiffCache(AutomationElement ell) {
        //    cachedState = ell.CaptureWpf();
        //    cachedElement = ell;
        //}
        //private bool DidGameDiff {
        //    get {
        //        BitmapImage bp = cachedElement.CaptureWpf();
        //        using (Stream s1 = bp.StreamSource) {
        //            using (Stream s2 = cachedState.StreamSource) {
        //                while (s1.CanRead && s2.CanRead) {
        //                    byte[] b1 = new byte[1];
        //                    s1.Read(b1, 0, 1);

        //                    byte[] b2 = new byte[1];
        //                    s2.Read(b2, 0, 1);

        //                    if (b1[0] != b2[0]) {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }

        //        return false;
        //    }
        //}
    }
}
