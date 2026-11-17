using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Library
{
    public class MainViewModel : ViewModelBase
    {
        private int fontSize = 14;
        public int FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }

        private string selectedTheme = "Светлая тема";
        public string SelectedTheme
        {
            get => selectedTheme;
            set
            {
                if (SetProperty(ref selectedTheme, value))
                {
                    OnPropertyChanged(nameof(WindowBackground));
                }
            }
        }

        private string selectedLanguage = "Русский";
        public string SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                if (SetProperty(ref selectedLanguage, value))
                {
                    OnPropertyChanged(nameof(TabMain));
                    OnPropertyChanged(nameof(TabPalette));
                    OnPropertyChanged(nameof(TabControls));
                    OnPropertyChanged(nameof(TabData));
                    OnPropertyChanged(nameof(TabBindings));
                    OnPropertyChanged(nameof(GroupPersonalization));
                    OnPropertyChanged(nameof(GroupFonts));
                    OnPropertyChanged(nameof(TextTheme));
                    OnPropertyChanged(nameof(TextFontSize));
                    OnPropertyChanged(nameof(FontType_));
                    OnPropertyChanged(nameof(Containers_));
                }
            }
        }

        private string selectedFont = "Segoe UI";
        public string SelectedFont
        {
            get => selectedFont;
            set
            {
                if (SetProperty(ref selectedFont, value))
                    OnPropertyChanged(nameof(CurrentFontFamily));
            }
        }

        public FontFamily CurrentFontFamily => new FontFamily(SelectedFont);

        public SolidColorBrush WindowBackground => SelectedTheme == "Темная тема"
            ? Brushes.DarkGray : Brushes.White;

        public SolidColorBrush WindowForeground => SelectedTheme == "Темная тема"
            ? Brushes.White : Brushes.DarkGray;

        public string TabMain => Translate("Main");
        public string TabPalette => Translate("Palette");
        public string TabControls => Translate("Controls");
        public string TabData => Translate("Data");
        public string TabBindings => Translate("Bindings");

        public string GroupPersonalization => Translate("Personalization");
        public string GroupFonts => Translate("Fonts");
        public string TextTheme => Translate("Theme");
        public string TextFontSize => Translate("FontSize");
        public string FontType_ => Translate("FontType");
        public string Containers_ => Translate("Containers");
        private string Translate(string key)
        {
            if (SelectedLanguage == "English") return TranslateEn(key);
            if (SelectedLanguage == "한국어") return TranslateKo(key);
            return TranslateRu(key);
        }

        private string TranslateRu(string k) => k switch
        {
            "Containers" => "Контейнеры компановки",
            "FontType" => "Тип шрифта",
            "Main" => "Основное",
            "Palette" => "Цветовая палитра",
            "Controls" => "Элементы управления",
            "Data" => "Управление данными",
            "Bindings" => "Типы привязок (Bindings)",
            "Personalization" => "Настройки персонализации",
            "Fonts" => "Настройки шрифтов",
            "Theme" => "Тема оформления:",
            "FontSize" => "Размер шрифта: ",
            _ => k
        };
        private string TranslateEn(string k) => k switch
        {
            "Containers" => "Layout containers",
            "FontType" => "Font Type",
            "Main" => "Main",
            "Palette" => "Color Palette",
            "Controls" => "UI Controls",
            "Data" => "Data Management",
            "Bindings" => "Binding Types",
            "Personalization" => "Personalization",
            "Fonts" => "Font Settings",
            "Theme" => "Theme:",
            "FontSize" => "Font Size: ",
            _ => k
        };
        private string TranslateKo(string k) => k switch
        {
            "Containers" => "레이아웃 컨테이너",
            "FontType" => "글꼴 유형",
            "Main" => "메인",
            "Palette" => "색상 팔레트",
            "Controls" => "UI 요소",
            "Data" => "데이터 관리",
            "Bindings" => "바인딩 유형",
            "Personalization" => "개인 설정",
            "Fonts" => "글꼴 설정",
            "Theme" => "테마:",
            "FontSize" => "글꼴 크기: ",
            _ => k
        };
        public ObservableCollection<ColorItem> BurgundyColors { get; } = new ObservableCollection<ColorItem>();
        public ObservableCollection<ColorItem> BlueColors { get; } = new ObservableCollection<ColorItem>();

        public ObservableCollection<DataItem> SourceTable { get; } = new ObservableCollection<DataItem>();
        public ObservableCollection<DataItem> TargetTable { get; } = new ObservableCollection<DataItem>();

        private DataItem selectedSourceItem;
        public DataItem SelectedSourceItem
        {
            get => selectedSourceItem;
            set => SetProperty(ref selectedSourceItem, value);
        }

        private DataItem copiedItem; 

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }

        public MainViewModel()
        {
            LoadColors();
            LoadInitialData();

            AddCommand = new RelayCommand(_ => SourceTable.Add(new DataItem { Name = "Новый элемент", Value = "0" }));
            DeleteCommand = new RelayCommand(_ => SourceTable.Remove(SelectedSourceItem), _ => SelectedSourceItem != null);
            EditCommand = new RelayCommand(_ => MessageBox.Show("Редактирование: " + SelectedSourceItem.Name), _ => SelectedSourceItem != null);

            CopyCommand = new RelayCommand(_ =>
            {
                if (SelectedSourceItem != null) copiedItem = new DataItem { Name = SelectedSourceItem.Name, Value = SelectedSourceItem.Value };
            }, _ => SelectedSourceItem != null);

            PasteCommand = new RelayCommand(_ =>
            {
                if (copiedItem != null) TargetTable.Add(new DataItem { Name = copiedItem.Name + " (Копия)", Value = copiedItem.Value });
            }, _ => copiedItem != null);
        }

        private void LoadColors()
        {
            BurgundyColors.Add(new ColorItem { ColorHex = "#FF800000", RGB = "128, 0, 0", Name = "Основной бордовый" });
            BurgundyColors.Add(new ColorItem { ColorHex = "#FF660000", RGB = "102, 0, 0", Name = "Тёмно-бордовый" });

            BlueColors.Add(new ColorItem { ColorHex = "#0078D4", RGB = "0, 120, 212", Name = "Основной синий" });
            BlueColors.Add(new ColorItem { ColorHex = "#0063B1", RGB = "0, 99, 177", Name = "Темно-синий" });
        }

        private void LoadInitialData()
        {
            SourceTable.Add(new DataItem { Name = "Элемент 1", Value = "100" });
            SourceTable.Add(new DataItem { Name = "Элемент 2", Value = "250" });
        }
    }
}
