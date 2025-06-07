using System; // Imports base .NET types like DateTime. // Импортирует базовые типы .NET, такие как DateTime.
using System.Collections.Generic; // Imports generic collections like List. // Импортирует обобщённые коллекции, такие как List.
using System.Linq; // Imports LINQ query capabilities. // Импортирует возможности LINQ-запросов.
using System.Text; // Imports text manipulation classes. // Импортирует классы для работы с текстом.
using System.Threading.Tasks; // Imports async programming support. // Импортирует поддержку асинхронного программирования.

namespace Sort_And_Scan.Classes // Declares the namespace for organizing related classes. // Объявляет пространство имён для организации связанных классов.
{
    public class DBRow // Defines the DBRow class representing a database row. // Определяет класс DBRow, представляющий строку базы данных.
    {
        public int ID { get; set; } // Unique identifier for the row. // Уникальный идентификатор строки.
        public string Item { get; set; } // Item code or name. // Код или название товара.
        public string Lot { get; set; } // Lot number for the item. // Номер партии товара.
        public string WH { get; set; } // Warehouse identifier. // Идентификатор склада.
        public string Package_Type { get; set; } // Type of packaging. // Тип упаковки.
        public string PickLocation_One { get; set; } // First pick location. // Первая зона отбора.
        public string PickLocation_Tow { get; set; } // Second pick location (likely typo: "Tow" should be "Two"). // Вторая зона отбора (возможно, опечатка: должно быть "Two").
        public string PickLocation_Three { get; set; } // Third pick location. // Третья зона отбора.
        public string Classification { get; set; } // Item classification. // Классификация товара.
        public string Status { get; set; } // Status of the row/item. // Статус строки/товара.
        public string Merge_Status { get; set; } // Merge status for the row. // Статус объединения строки.
        public string RowColor { get; set; } // Row color for UI highlighting. // Цвет строки для выделения в интерфейсе.
        public string Customer { get; set; } // Customer name or code. // Имя или код клиента.
        public DateTime Date { get; set; } // Associated date for the row. // Связанная дата для строки.
        public string DateCode { get; set; } // Date code as a string. // Датировочный код в виде строки.
        public double Qty { get; set; } // Quantity of the item. // Количество товара.
    }

    public class LotPerPL // Defines the LotPerPL class for lot info per pick location. // Определяет класс LotPerPL для информации о партии по зоне отбора.
    {
        public string Lot { get; set; } // Lot number. // Номер партии.
        public string WH { get; set; } // Warehouse identifier. // Идентификатор склада.
        public string Sec_WH { get; set; } // Secondary warehouse identifier. // Идентификатор второго склада.
        public int Qty { get; set; } // Quantity in this lot/location. // Количество в данной партии/зоне.
        public string PL { get; set; } // Pick location. // Зона отбора.
        public string Remark { get; set; } // Additional remarks. // Дополнительные примечания.
        public string Remark_Qty { get; set; } // Remarks about quantity. // Примечания по количеству.
        public string Kit_Status { get; set; } // Kit status. // Статус комплекта.
        public string Row_Color { get; set; } // Row color for UI. // Цвет строки для интерфейса.
        public string Size { get; set; } // Size information. // Информация о размере.
        public string DateCode { get; set; } // Date code. // Датировочный код.
        public string Cart { get; set; } // Cart identifier. // Идентификатор тележки.
        public string Slot { get; set; } // Slot identifier. // Идентификатор слота.
        public string Tray { get; set; } // Tray identifier. // Идентификатор лотка.
        public string Customer { get; set; } // Customer name or code. // Имя или код клиента.
        public string PN { get; set; } // Part number. // Номер детали.
        public bool PickEnabled { get; set; } // Is picking enabled. // Разрешён ли отбор.
        public int DC_Year { get; set; } // Date code year. // Год датировочного кода.
        public int DC_Month { get; set; } // Date code month. // Месяц датировочного кода.
    }

    public class Item // Defines the Item class for work order items. // Определяет класс Item для позиций производственного заказа.
    {
        public string WO { get; set; } // Work order number. // Номер производственного заказа.
        public string PN { get; set; } // Part number. // Номер детали.
        public string PL { get; set; } // Pick location. // Зона отбора.
        public int Qty { get; set; } // Quantity required. // Требуемое количество.
        public int Demaned { get; set; } // Demand (likely typo: should be "Demand"). // Потребность (вероятно, опечатка: должно быть "Demand").
    }

    public class SiplaceRow // Defines the SiplaceRow class for Siplace machine setup. // Определяет класс SiplaceRow для настройки машины Siplace.
    {
        public string Item { get; set; } // Item code or name. // Код или название товара.
        public string Station { get; set; } // Station name. // Название станции.
        public string StationNumber { get; set; } // Station number. // Номер станции.
        public string Location { get; set; } // Location identifier. // Идентификатор местоположения.
        public string Track { get; set; } // Track identifier. // Идентификатор дорожки.
        public int Division { get; set; } // Division number. // Номер секции.
        public string Tower { get; set; } // Tower identifier. // Идентификатор башни.
        public string Level { get; set; } // Level identifier. // Идентификатор уровня.
        public string Setup { get; set; } // Setup information. // Информация о настройке.
        public string PickLocation_One { get; set; } // First pick location. // Первая зона отбора.
        public string PickLocation_Tow { get; set; } // Second pick location (likely typo: "Tow" should be "Two"). // Вторая зона отбора (возможно, опечатка: должно быть "Two").
        public string PickLocation_Three { get; set; } // Third pick location. // Третья зона отбора.
        public int numOfStation { get; set; } // Number of stations. // Количество станций.
    }

    public class FinishedRow // Defines the FinishedRow class for completed operations. // Определяет класс FinishedRow для завершённых операций.
    {
        public string PL { get; set; } // Pick location. // Зона отбора.
        public string WH { get; set; } // Warehouse identifier. // Идентификатор склада.
        public string Customer { get; set; } // Customer name or code. // Имя или код клиента.
        public string Line { get; set; } // Production line. // Производственная линия.
        public string Worker { get; set; } // Worker name or ID. // Имя или идентификатор работника.
        public string Total_Scanned { get; set; } // Total scanned quantity. // Общее количество отсканированного.
        public DateTime Time { get; set; } // Time of completion. // Время завершения.
    }

    public class WhKittingModel // Defines the WhKittingModel class for warehouse kitting operations. // Определяет класс WhKittingModel для операций комплектации на складе.
    {
        public int Id { get; set; } // Unique identifier. // Уникальный идентификатор.
        public string Lot { get; set; } // Lot number. // Номер партии.
        public string Item { get; set; } // Item code or name. // Код или название товара.
        public string Status { get; set; } // Status of the kitting operation. // Статус операции комплектации.
        public string Package { get; set; } // Package type. // Тип упаковки.
        public string PL { get; set; } // Pick location. // Зона отбора.
        public string Employee_Name { get; set; } // Employee name. // Имя сотрудника.
        public int Total_First { get; set; } // Total first quantity. // Основное количество.
        public string Customer { get; set; } // Customer name or code. // Имя или код клиента.
        public int Total_Backup { get; set; } // Total backup quantity. // Резервное количество.
        public DateTime Date { get; set; } // Date of the operation. // Дата операции.
    }

    public class BAAN // Defines the BAAN class. // Определяет класс BAAN.
    {
        public string Pn { get; set; } // Property for part number; stores the part number from the BAAN system. // Свойство для номера детали; хранит номер детали из системы BAAN.
        public string Type { get; set; } // Property for type; stores the type of the BAAN record or item. // Свойство для типа; хранит тип записи или элемента BAAN.
    }
}
// The BAAN class is a simple data model for storing information from the BAAN system, such as part number and type. // Класс BAAN — это простая модель данных для хранения информации из системы BAAN, такой как номер детали и тип.