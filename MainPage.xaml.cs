using System;
using System.Xml.Linq;
using System.Xml.Xsl;


namespace Laba_2;

public partial class MainPage : ContentPage
{
    public string selectedFilePath = "";
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
        public bool IsResidingInDormitory { get; set; }
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
        //для гріда сєрча
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
            LoadPicker(DormitoryNumberPicker, "DormitoryNumber");
            LoadPicker(RoomNumberPicker, "RoomNumber");
            LoadPicker(ContractEndDatePicker, "ContractEndDate");
            await DisplayAlert($"Ви обрали файл: {result.FileName}", "", "OK");
        }
        else
        {
            await DisplayAlert("Відміна вибору", "Ви не обрали файл. Будь ласка, оберіть файл", "OK");
        }
    }
   
}