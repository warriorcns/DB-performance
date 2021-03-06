﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.18052
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBaseClient {
    using System;
    
    
    /// <summary>
    ///   Klasa zasobu wymagająca zdefiniowania typu do wyszukiwania zlokalizowanych ciągów itd.
    /// </summary>
    // Ta klasa została automatycznie wygenerowana za pomocą klasy StronglyTypedResourceBuilder
    // przez narzędzie, takie jak ResGen lub Visual Studio.
    // Aby dodać lub usunąć członka, edytuj plik .ResX, a następnie ponownie uruchom ResGen
    // z opcją /str lub ponownie utwórz projekt VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Queries {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Queries() {
        }
        
        /// <summary>
        /// Zwraca buforowane wystąpienie ResourceManager używane przez tę klasę.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DataBaseClient.Queries", typeof(Queries).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Zastępuje właściwość CurrentUICulture bieżącego wątku dla wszystkich
        ///   przypadków przeszukiwania zasobów za pomocą tej klasy zasobów wymagającej zdefiniowania typu.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.deleteAllData.
        /// </summary>
        internal static string deleteData {
            get {
                return ResourceManager.GetString("deleteData", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT MAX(id) FROM @tableName.
        /// </summary>
        internal static string GetMaxIDEntry {
            get {
                return ResourceManager.GetString("GetMaxIDEntry", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.insertCategory @name.
        /// </summary>
        internal static string insertCategory {
            get {
                return ResourceManager.GetString("insertCategory", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.insertPesel @number.
        /// </summary>
        internal static string insertPesel {
            get {
                return ResourceManager.GetString("insertPesel", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu INSERT into dbo.Categories (id, number) values (@id, @number).
        /// </summary>
        internal static string InsertRandomCategories {
            get {
                return ResourceManager.GetString("InsertRandomCategories", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu INSERT into dbo.Pesels (id, pesel) values (@id, @pesel).
        /// </summary>
        internal static string InsertRandomPesels {
            get {
                return ResourceManager.GetString("InsertRandomPesels", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu INSERT into dbo.Pesels (id, name) values (@id, @thing).
        /// </summary>
        internal static string InsertRandomThings {
            get {
                return ResourceManager.GetString("InsertRandomThings", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu INSERT into dbo.Users (id, name) values (@id, @name).
        /// </summary>
        internal static string InsertRandomUsers {
            get {
                return ResourceManager.GetString("InsertRandomUsers", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.insertThing @name.
        /// </summary>
        internal static string insertThing {
            get {
                return ResourceManager.GetString("insertThing", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.insertUser @name.
        /// </summary>
        internal static string insertUser {
            get {
                return ResourceManager.GetString("insertUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.populateDB 
        ///   @userMainID
        ///  ,@userName
        ///  ,@thingMainID
        ///  ,@peselMainID
        ///  ,@categoryMainID
        ///  ,@thingName
        ///  ,@number
        ///  ,@categoryName
        ///  ,@connectTableMainID.
        /// </summary>
        internal static string populateDatabaseProcedure {
            get {
                return ResourceManager.GetString("populateDatabaseProcedure", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.selectCategory @rowNumber.
        /// </summary>
        internal static string selectCategory {
            get {
                return ResourceManager.GetString("selectCategory", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.selectPesel @rowNumber.
        /// </summary>
        internal static string selectPesel {
            get {
                return ResourceManager.GetString("selectPesel", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.selectThing @rowNumber.
        /// </summary>
        internal static string selectThing {
            get {
                return ResourceManager.GetString("selectThing", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.selectUser @rowNumber.
        /// </summary>
        internal static string selectUser {
            get {
                return ResourceManager.GetString("selectUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.updateCategory @name , @rowNumber.
        /// </summary>
        internal static string updateCategory {
            get {
                return ResourceManager.GetString("updateCategory", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.updatePesels @name , @rowNumber , @randomNumber.
        /// </summary>
        internal static string updatePesels {
            get {
                return ResourceManager.GetString("updatePesels", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.updateThing @name , @rowNumber.
        /// </summary>
        internal static string updateThing {
            get {
                return ResourceManager.GetString("updateThing", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu EXECUTE dbo.updateUser @name , @rowNumber.
        /// </summary>
        internal static string updateUser {
            get {
                return ResourceManager.GetString("updateUser", resourceCulture);
            }
        }
    }
}
