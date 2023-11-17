using System;
using System.Xml.Linq;
using System.Xml.Xsl;
using Laba_2;


namespace Laba_2;

public partial class MainPage : ContentPage
{
    public string selectedFilePath = "";
    List<string> filterDormitory = new List<string>();
    public ISearch LINQ = new LINQ();
    ScrollView FrameScrollView = new ScrollView { Orientation = ScrollOrientation.Horizontal };
    StackLayout FrameStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
    public MainPage()
    {
        InitializeComponent();
    }
    void OnNumberEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        entry.Text = new String(entry.Text.Where(Char.IsDigit).ToArray());
    }
    void OnLetterEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        entry.Text = new String(entry.Text.Where(c => Char.IsLetter(c) || Char.IsWhiteSpace(c)).ToArray());
    }
    private void LoadPicker(Picker picker, string elementPath)
    {
            XDocument doc = XDocument.Load(selectedFilePath);
            var elements = doc.Descendants("dormitory")
                .Select(d =>
                {
                    var pathParts = elementPath.Split('/');
                    var currentElement = d.Element("PersonalInformation");
                    foreach (var part in pathParts)
                    {
                        currentElement = currentElement.Element(part);
                    }
                    return (string)currentElement;
                })
                .Distinct()
                .ToList();
            picker.ItemsSource = elements;
     }

    public class Dormitory
    {
        public string FullName { get; set; }
        public int DormitoryNumber { get; set; }
        public int Floor { get; set; }
        public string RoomNumber { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string IsResidingInDormitory { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public int Course { get; set; }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e, Label label, View view)
    {
        if (e.Value)
        {
            label.IsEnabled = e.Value;
            view.IsEnabled = e.Value;
        }
        else
        {
            switch (view)
            {
                case Entry entry:
                    entry.Text = string.Empty;
                    break;
                case Picker picker:
                    picker.SelectedItem = null;
                    break;
            }
            label.IsEnabled = false;
            view.IsEnabled = false;
        }
    }
    private void FullNameBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, FullNameLabel, FullNameEntry);
    }

    private void FloorBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, FloorLabel, FloorEntry);
    }
    private void FacultyBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, FacultyLabel, FacultyPicker);
    }

    private void CourseBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, CourseLabel, CoursePicker);
    }

    private void DepartmentBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, DepartmentLabel, DepartmentPicker);
    }

    private void DormitoryNumberBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, DormitoryNumberLabel, DormitoryNumberPicker);
    }

    private void RoomNumberBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, RoomNumberLabel, RoomNumberPicker);
    }

    private void ContractEndDateBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, ContractEndDateLabel, ContractEndDatePicker);
    }

    private void IsResidingInDormitoryBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox_CheckedChanged(sender, e, IsResidingInDormitoryLabel, IsResidingInDormitoryPicker);
    }
    
    private void LINQBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        SearchButton_Enable();
        if (e.Value)
        {
            LINQLabel.TextColor = Color.FromRgb(50, 205, 50);
            DOMBox.IsEnabled = false;
            SAXBox.IsEnabled = false;
        }
        else
        {
            LINQLabel.TextColor = Color.FromRgb(255, 0, 0);
            DOMBox.IsEnabled = true;
            SAXBox.IsEnabled = true;
        }
    }
    private void DOMBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        SearchButton_Enable();
        if (e.Value)
        {
            DOMLabel.TextColor = Color.FromRgb(50, 205, 50);
            LINQBox.IsEnabled = false;
            SAXBox.IsEnabled = false;
        }
        else
        {
            DOMLabel.TextColor = Color.FromRgb(255, 0, 0);
            LINQBox.IsEnabled = true;
            SAXBox.IsEnabled = true;
        }
    }
    private void SAXBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        SearchButton_Enable();
        if (e.Value)
        {
            SAXLabel.TextColor = Color.FromRgb(50, 205, 50);
            LINQBox.IsEnabled = false;
            DOMBox.IsEnabled = false;
        }
        else
        {
            LINQBox.IsEnabled = true;
            DOMBox.IsEnabled = true;
            SAXLabel.TextColor = Color.FromRgb(255, 0, 0);
        }
    }
    private void SearchButton_Enable()
    {
        if ((SAXBox.IsChecked == false) && (LINQBox.IsChecked == false) && (DOMBox.IsChecked == false))
        {
            SearchButton.IsEnabled = false;
        }
        else
        {
            SearchButton.IsEnabled = true;
        }
    }
    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        FullNameBox.IsChecked = false;
        FloorBox.IsChecked = false;
        DormitoryNumberBox.IsChecked = false;
        RoomNumberBox.IsChecked = false;
        ContractEndDateBox.IsChecked = false;
        IsResidingInDormitoryBox.IsChecked = false;
        FacultyBox.IsChecked = false;
        DepartmentBox.IsChecked = false;
        CourseBox.IsChecked = false;
        LINQBox.IsChecked = false;
        DOMBox.IsChecked = false;
        SAXBox.IsChecked = false;
        FrameStackLayout.Children.Clear();
        FrameScrollView.Content = null;
        Main.Children.Remove(FrameScrollView);
    }
    private async void ConvertToHTMLButton_Clicked(object sender, EventArgs e)
    {
        try
        {
           if(selectedFilePath != string.Empty)
            {
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load("C:\\Visual Studio project\\Laba 2\\Dormitories.xsl");
                xslt.Transform(selectedFilePath, "C:\\Visual Studio project\\Laba 2\\Dormitories.html");
                await DisplayAlert("Успішно", "Конвертація пройшла успішно", "OK");
            }
            else
            {
                await DisplayAlert("Помилка", "Будь ласка, оберіть файл", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", $"Помилка конвертації: {ex.Message}", "OK");
        }
    }
    private async void OnExitButtonClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Підтвердіть вихід", "Ви дійсно хочете вийти?", "Так", "Ні");
        if (answer)
        {
            System.Environment.Exit(0);
        }
    }
    private async void OnPickFileButtonClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".xml" } }
                })
        });
        if (result != null)
        {
            selectedFilePath = result.FullPath;
            LoadPicker(FacultyPicker, "AcademicDetails/Faculty");
            LoadPicker(DepartmentPicker, "AcademicDetails/Department");
            LoadPicker(CoursePicker, "AcademicDetails/Course");
            LoadPicker(DormitoryNumberPicker, "DormitoryNumber");
            LoadPicker(RoomNumberPicker, "RoomNumber");
            LoadPicker(ContractEndDatePicker, "ContractEndDate");
            LoadPicker(IsResidingInDormitoryPicker, "IsResidingInDormitory");
            await DisplayAlert($"Ви обрали файл: {result.FileName}", "", "OK");
        }
        else
        {
            await DisplayAlert("Відміна вибору", "Ви не обрали файл. Будь ласка, оберіть файл", "OK");
        }
    }
    private void AddToFilterDormitory(string text, bool isEnabled)
    {
        if (!string.IsNullOrEmpty(text) && isEnabled)
        {
            filterDormitory.Add(text);
        }
    }
    private void SearchButton_Clicked(object sender, EventArgs e)
    {
        FrameStackLayout.Children.Clear();
        FrameScrollView.Content = null;
        Main.Children.Remove(FrameScrollView);
        AddToFilterDormitory(FullNameEntry.Text, FullNameEntry.IsEnabled);
        AddToFilterDormitory((string)DormitoryNumberPicker.SelectedItem, DormitoryNumberPicker.IsEnabled);
        AddToFilterDormitory(FloorEntry.Text, FloorEntry.IsEnabled);
        AddToFilterDormitory((string)RoomNumberPicker.SelectedItem, RoomNumberPicker.IsEnabled);
        AddToFilterDormitory((string)ContractEndDatePicker.SelectedItem, ContractEndDatePicker.IsEnabled);
        AddToFilterDormitory((string)IsResidingInDormitoryPicker.SelectedItem, IsResidingInDormitoryPicker.IsEnabled);
        AddToFilterDormitory((string)FacultyPicker.SelectedItem, FacultyPicker.IsEnabled);
        AddToFilterDormitory((string)CoursePicker.SelectedItem, CoursePicker.IsEnabled);
        AddToFilterDormitory((string)DepartmentPicker.SelectedItem, DepartmentPicker.IsEnabled);
        if (LINQBox.IsChecked)
        {
            List<Dormitory> dormitories = LINQ.Search(selectedFilePath, filterDormitory);
            foreach (Dormitory dormitory in dormitories)
            {
                Frame frame = new Frame
                {
                    BorderColor = Colors.Black,
                    BackgroundColor = Colors.Beige,
                    CornerRadius = 15,
                    Margin = new Thickness(0, 0, 5, 0),
                    Content = new VerticalStackLayout
                    {
                        new HorizontalStackLayout
                        {
                            new Label { Text = "П.І.П:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.FullName, FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Номер гуртожитку:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.DormitoryNumber.ToString(), FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Поверх:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.Floor.ToString(), FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Номер кімнати:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.RoomNumber.ToString(), FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Дата закінчення договору:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.ContractEndDate.ToString("yyyy-MM-dd"), FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Чи проживає у гуртожитку:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.IsResidingInDormitory.ToString(), FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Факультет:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.Faculty.ToString(), FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Кафедра:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.Department.ToString(), FontSize = 13 }
                        },
                        new HorizontalStackLayout
                        {
                            new Label { Text = "Курс:", FontFamily = "Impact", FontSize = 13, Padding = new Thickness(0, 0, 5, 0) },
                            new Label { Text = dormitory.Course.ToString(), FontSize = 13 }
                        }
                    }
                };
                FrameStackLayout.Children.Add(frame);
            }
            FrameScrollView.Content = FrameStackLayout;
            Main.Children.Add(FrameScrollView);
        }
        filterDormitory.Clear();
    } 
}