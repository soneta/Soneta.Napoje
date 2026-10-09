using Soneta.Business.UI;
using Soneta.Napoje.UI;

[assembly: FolderView("Handel/Napoje", // wymagane: Ścieżka folderu
    Caption = "Napoje", // niewymagane - Tytuł, jeśli nie podane, pobrane będzie ze ścieżki powyżej
    Priority = 0, // opcjonalne: Im mniejsza wartość, tym większy priorytet
    Description = "Moduł dotyczący napojów", // opcjonalne: Opis poniżej tytułu kafla
    IconName = "towar" // opcjonalne: nazwa ikonki
)]

[assembly: FolderView("Handel/Napoje/Napoje",
    Priority = 0,
    Description = "Towary oznaczone jako napoje",
    TableName = "Napoje", // Tabela, której widok będzie prezentowany
    ViewType = typeof(NapojeViewInfo), // ViewInfo, które będzie użyte do wyświetlenia listy
    IconName = "towar"
)]

[assembly: FolderView("Handel/Napoje/KategorieNapojow",
    Priority = 1,
    Description = "Kategorie napojów",
    TableName = "KategorieNapoj",
    ViewType = typeof(KategorieNapojowViewInfo),
    IconName = "towar"
)]

[assembly: FolderView("Handel/Napoje/ProducenciNapojow",
    Priority = 2,
    Description = "Producenci napojów",
    TableName = "ProducenciNapoj",
    ViewType = typeof(ProducenciNapojowViewInfo),
    IconName = "towar"
)]
