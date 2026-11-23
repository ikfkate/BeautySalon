using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL.Services;
using BLL.DTO;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private ProcedureService _procedureService = new ProcedureService();
        private AppointmentService _appointmentService = new AppointmentService();

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> ChartLabels { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            // Подписываемся на событие из BookingVie
            BookingPage.BookingCompleted += BookingPage_BookingCompleted;
        }

        // Метод для скрытия всех страниц 
        private void HideAllPages()
        {
            HomePage.Visibility = Visibility.Collapsed;
            CabinetPage.Visibility = Visibility.Collapsed;
            ReportsPage.Visibility = Visibility.Collapsed;
            BookingPage.Visibility = Visibility.Collapsed;
            ManagementPage.Visibility = Visibility.Collapsed;
        }

        // Главная
        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            HomePage.Visibility = Visibility.Visible;
            PageTitle.Text = "Главная";
        }

        // Личный кабинет
        private void BtnCabinet_Click(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            CabinetPage.Visibility = Visibility.Visible;
            PageTitle.Text = "Личный кабинет";
        }

        // Управление
        private void BtnManagement_Click(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            ManagementPage.Visibility = Visibility.Visible;
            PageTitle.Text = "Управление";
            LoadProcedures();
        }

        // Отчеты 
        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            ReportsPage.Visibility = Visibility.Visible;
            PageTitle.Text = "Отчеты";

            LoadReportData(); // Загружаем данные при открытии вкладки
        }

        // Кнопка "Обновить данные" 
        private void BtnUpdateReports_Click(object sender, RoutedEventArgs e)
        {
            LoadReportData();
        }
        // Метод загрузки данных из БД (через BLL)
        //private void LoadReportData()
        //{
        //    try
        //    {
        //        // Вызываем метод из BLL. Он вернет List<ProcedureDTO>
        //        var procedures = _procedureService.GetAllProcedures();

        //        // Привязываем к таблице. Так как это список объектов, DataGrid 
        //        // автоматически создаст колонки на основе свойств класса ProcedureDTO
        //        AppointmentsGrid.ItemsSource = procedures;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        // Для записей
        //private void LoadReportData()
        //{
        //    try
        //    {
        //        var appointments = _appointmentService.GetAllAppointments();

        //        AppointmentsGrid.ItemsSource = appointments;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        private void LoadReportData()
        {
            try
            {
                // Загружаем данные в таблицу (как было)
                //var appointments = _appointmentService.GetAllAppointments();
                //AppointmentsGrid.ItemsSource = appointments;

                // Берем процедуры для графика
                var procedures = _procedureService.GetAllProcedures();

                // Создаем два простых списка: для названий и для цен
                List<string> titles = new List<string>();
                ChartValues<double> prices = new ChartValues<double>();

                foreach (var p in procedures)
                {
                    titles.Add(p.Title);                  // Добавляем название
                    prices.Add((double)p.Price);          // Добавляем цену (переводим decimal в double для графика)
                }

                // Создаем столбцы и сразу передаем в них цены
                var columnSeries = new ColumnSeries
                {
                    Title = "Цена",
                    Values = prices,
                    Fill = (System.Windows.Media.Brush)Application.Current.Resources["RoseColor"] // Красим в ваш розовый цвет
                };

                // Настраиваем график (без сложных привязок)
                ProcedureChart.Series = new SeriesCollection { columnSeries };
                ProcedureChart.AxisX[0].Labels = titles; // Передаем названия процедур на ось X
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // Кнопка "Записаться на процедуру"
        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            BookingPage.Visibility = Visibility.Visible;
            PageTitle.Text = "Запись на процедуру";
        }

        private void BookingPage_BookingCompleted(object? sender, EventArgs e)
        {
            // Возвращаемся на главную страницу
            HideAllPages();
            HomePage.Visibility = Visibility.Visible;
            PageTitle.Text = "Главная";

            // Сбрасываем экран записи на первый шаг для следующего раза
            BookingPage.ResetView();
        }


        // Метод загрузки процедур в таблицу
        private void LoadProcedures()
        {
            try
            {
                ProceduresGrid.ItemsSource = _procedureService.GetAllProcedures();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для очистки полей формы
        private void ClearForm()
        {
            TxtTitle.Clear();
            TxtCategory.Clear();
            TxtDuration.Clear();
            TxtPrice.Clear();
            TxtDescription.Clear();
            ProceduresGrid.SelectedItem = null;
        }

        // Добавление
        private void BtnAddProcedure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Собираем данные из формы
                var newProcedure = new ProcedureDTO
                {
                    Title = TxtTitle.Text,
                    Category = TxtCategory.Text,
                    DurationMinutes = int.TryParse(TxtDuration.Text, out int dur) ? dur : 0,
                    Price = decimal.TryParse(TxtPrice.Text, out decimal pr) ? pr : 0,
                    Description = TxtDescription.Text
                };

                // Отдаем в BLL
                _procedureService.CreateProcedure(newProcedure);

                MessageBox.Show("Процедура успешно добавлена!", "Успех");
                LoadProcedures();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обновление
        private void BtnUpdateProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (ProceduresGrid.SelectedItem is not ProcedureDTO selected)
            {
                MessageBox.Show("Выберите процедуру в таблице для изменения.", "Внимание");
                return;
            }

            try
            {
                selected.Title = TxtTitle.Text;
                selected.Category = TxtCategory.Text;
                selected.DurationMinutes = int.TryParse(TxtDuration.Text, out int dur) ? dur : 0;
                selected.Price = decimal.TryParse(TxtPrice.Text, out decimal pr) ? pr : 0;
                selected.Description = TxtDescription.Text;

                _procedureService.UpdateProcedure(selected);

                MessageBox.Show("Процедура обновлена!", "Успех");
                LoadProcedures();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Удаление 
        private void BtnDeleteProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (ProceduresGrid.SelectedItem is not ProcedureDTO selected)
            {
                MessageBox.Show("Выберите процедуру в таблице для удаления.", "Внимание");
                return;
            }

            var result = MessageBox.Show($"Удалить процедуру \"{selected.Title}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _procedureService.DeleteProcedure(selected.Id);
                    MessageBox.Show("Процедура удалена!", "Успех");
                    LoadProcedures();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ProceduresGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProceduresGrid.SelectedItem is BLL.DTO.ProcedureDTO selected)
            {
                TxtTitle.Text = selected.Title;
                TxtCategory.Text = selected.Category;
                TxtDuration.Text = selected.DurationMinutes.ToString();
                TxtPrice.Text = selected.Price.ToString();
                TxtDescription.Text = selected.Description;
            }
        }
    }
}