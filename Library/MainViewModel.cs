using CodingSeb.Localization;
using System;
using System.Collections;
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

        private string selectedTheme = "Light";
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
                    Loc.Instance.CurrentLanguage = value switch
                    {
                        "English" => "en",
                        "한국어" => "ko",
                        _ => "ru"
                    };
                    OnPropertyChanged(nameof(SelectedTheme));
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

        public SolidColorBrush WindowBackground => SelectedTheme == "Dark"
            ? Brushes.DarkGray : Brushes.White;

        
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

        private List<DataItem> copiedItems = new();

        public IList SelectedItems { get; set; } = new List<DataItem>();
        public MainViewModel()
        {
            LoadColors();
            LoadInitialData();

            AddCommand = new RelayCommand(_ => SourceTable.Add(new DataItem { Name = "Новый элемент", Value = "0" }));
            DeleteCommand = new RelayCommand(_ => SourceTable.Remove(SelectedSourceItem), _ => SelectedSourceItem != null);
            EditCommand = new RelayCommand(_ => MessageBox.Show("Редактирование: " + SelectedSourceItem.Name), _ => SelectedSourceItem != null);


            CopyCommand = new RelayCommand(_ =>
            {
                copiedItems = SelectedItems.Cast<DataItem>().ToList();
            }, _ => SelectedItems.Count > 0);

            PasteCommand = new RelayCommand(_ =>
            {
                foreach (var item in copiedItems)  
                    TargetTable.Add(new DataItem { Name = item.Name, Value = item.Value });
            }, _ => copiedItems.Count > 0);
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

        private string sourceOnlyText = string.Empty;
        public string SourceOnlyText
        {
            get => sourceOnlyText;
            set => SetProperty(ref sourceOnlyText, value);
        }
    }
}
