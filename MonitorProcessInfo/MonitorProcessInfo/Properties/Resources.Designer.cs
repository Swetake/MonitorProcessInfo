﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitorProcessInfo.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MonitorProcessInfo.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Invalid time format に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ExceptionMsgInvalidTimeFormat {
            get {
                return ResourceManager.GetString("ExceptionMsgInvalidTimeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Key not found in Dict.  に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ExceptionMsgKeyNotFoundInDict {
            get {
                return ResourceManager.GetString("ExceptionMsgKeyNotFoundInDict", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Order Shared memory file already exists or cannot be created. This activity should be used only once.  に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ExceptionMsgOrderMemFileAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionMsgOrderMemFileAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   The process has same ID but not same Starttime. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ExceptionMsgSameIdButNotSameStarttime {
            get {
                return ResourceManager.GetString("ExceptionMsgSameIdButNotSameStarttime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   MPI_Order に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string OrderMemFileName {
            get {
                return ResourceManager.GetString("OrderMemFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   . に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string PerfomanceCounterMachineName {
            get {
                return ResourceManager.GetString("PerfomanceCounterMachineName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   _Total に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string PerformaceCounterInstanceName {
            get {
                return ResourceManager.GetString("PerformaceCounterInstanceName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Processor に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string PerformanceCounterCategoryName {
            get {
                return ResourceManager.GetString("PerformanceCounterCategoryName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   % Processor Time に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string PerformanceCounterCounterName {
            get {
                return ResourceManager.GetString("PerformanceCounterCounterName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   MPI_ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string PrefixMemFileName {
            get {
                return ResourceManager.GetString("PrefixMemFileName", resourceCulture);
            }
        }
    }
}