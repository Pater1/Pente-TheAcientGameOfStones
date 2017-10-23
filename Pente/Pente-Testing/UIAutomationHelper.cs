using System;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

namespace Pente_Testing {
    public static class UIAutomationHelper {
        public static T FindVisualChild<T>(FrameworkElement depObj, Func<FrameworkElement, bool> queary) where T : FrameworkElement {
            if (depObj != null) {
                T ret = depObj as T;
                if (ret != null) {
                    return ret;
                }

                Type t = depObj.GetType();
                PropertyInfo pfi = t.GetProperty("Children");
                if (pfi != null) {
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
        public static void Click(object logicHolder, string funcReflect, object clicked) {
            Type t = logicHolder.GetType();
            MethodInfo meth = t.GetMethod(funcReflect, BindingFlags.NonPublic | BindingFlags.Instance);
            meth.Invoke(logicHolder, new object[] { clicked, null });
        }
    }
}
