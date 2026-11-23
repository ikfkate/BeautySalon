using System;
using System.Collections.Generic;
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


namespace WpfApp2
{
    public partial class BookingView : UserControl
    {
        private int currentStep = 1;
        private int maxReachedStep = 1; // максимальный достигнутый шаг
        private string selectedProcedure = "";
        private string selectedMaster = ""; 
        private string selectedTime = "";   
        private bool isItemSelected = false;
        private Button lastSelectedButton = null;
        public event EventHandler? BookingCompleted; // Событие завершения записи 

        public BookingView()
        {
            InitializeComponent();
            UpdateStepUI();
        }

        // Метод для скрытия всех шагов
        private void HideAllSteps()
        {
            Step1_Categories.Visibility = Visibility.Collapsed;
            Step2_Procedures.Visibility = Visibility.Collapsed;
            Step3_Masters.Visibility = Visibility.Collapsed;
            Step4_Time.Visibility = Visibility.Collapsed;
            Step5_Confirm.Visibility = Visibility.Collapsed;
        }

        // Обработчик кнопки "Далее"
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep < 5)
            {
                currentStep++;
                // Обновляем максимальный достигнутый шаг
                if (currentStep > maxReachedStep)
                {
                    maxReachedStep = currentStep;
                }
                UpdateStepUI();
            }
            else
            {
                MessageBox.Show("Вы успешно записаны!", "Успех");
                BookingCompleted?.Invoke(this, EventArgs.Empty);
            }
        }


        // Обработчик кнопки "Назад"
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;
                isItemSelected = false;
                UpdateStepUI();
            }
        }

        // Обновление интерфейса в зависимости от шага
        private void UpdateStepUI()
        {
            HideAllSteps();
            BtnBack.Visibility = currentStep > 1 ? Visibility.Visible : Visibility.Collapsed;

            isItemSelected = false;
            BtnNext.IsEnabled = false;
            BtnNext.Visibility = Visibility.Collapsed;

            switch (currentStep)
            {
                case 1:
                    StepTitle.Text = "Шаг 1: Выберите категорию";
                    Step1_Categories.Visibility = Visibility.Visible;
                    BtnNext.Content = "Далее →";
                    break;
                case 2:
                    StepTitle.Text = "Шаг 2: Выберите процедуру";
                    Step2_Procedures.Visibility = Visibility.Visible;
                    BtnNext.Content = "Далее →";
                    break;
                case 3:
                    StepTitle.Text = "Шаг 3: Выберите мастера";
                    Step3_Masters.Visibility = Visibility.Visible;
                    BtnNext.Content = "Далее →";
                    break;
                case 4:
                    StepTitle.Text = "Шаг 4: Выберите время";
                    Step4_Time.Visibility = Visibility.Visible;
                    BtnNext.Content = "Далее →";
                    break;
                case 5:
                    StepTitle.Text = "Шаг 5: Подтверждение";
                    Step5_Confirm.Visibility = Visibility.Visible;
                    BtnNext.Content = "Подтвердить";
                    BtnNext.IsEnabled = true;
                    BtnNext.Visibility = Visibility.Visible;
                    ConfirmDetails.Text = "Процедура: " + selectedProcedure;
                    ConfirmMaster.Text = "Мастер: " + selectedMaster;
                    ConfirmTime.Text = "Дата и время: 15 октября, " + selectedTime;
                    break;
            }

            // Обновляем вкладки после смены шага
            UpdateTabs();
        }


        // Обновление состояния вкладок
        private void UpdateTabs()
        {
            // Собираем все вкладки в массив для удобства
            Button[] tabs = { TabStep1, TabStep2, TabStep3, TabStep4, TabStep5 };

            for (int i = 0; i < tabs.Length; i++)
            {
                int stepNum = i + 1;
                if (stepNum == currentStep)
                {
                    tabs[i].Tag = "Active"; // Текущий шаг
                }
                else if (stepNum <= maxReachedStep)
                {
                    tabs[i].Tag = "Completed"; // Пройденный шаг 
                }
                else
                {
                    tabs[i].Tag = "Disabled"; // Будущий шаг 
                }
            }
        }

        // Клик по вкладке
        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedTab &&
                int.TryParse(clickedTab.CommandParameter?.ToString(), out int targetStep))
            {
                // Разрешаем переход только на текущий или пройденные шаги
                if (targetStep <= maxReachedStep && targetStep != currentStep)
                {
                    currentStep = targetStep;
                    UpdateStepUI();
                }
            }
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, на которую нажали
            Button clickedButton = sender as Button;

            // Если до этого была выбрана другая кнопка - возвращаем ей обычный фон
            if (lastSelectedButton != null)
            {
                lastSelectedButton.ClearValue(Button.BackgroundProperty);
            }

            // Подсвечиваем новую выбранную кнопку (цвет LightRose из палитры)
            clickedButton.Background = (SolidColorBrush)Application.Current.Resources["LightRose"];
            lastSelectedButton = clickedButton; // Запоминаем её

            // Включаем кнопку "Далее"
            isItemSelected = true;
            BtnNext.IsEnabled = true;
            BtnNext.Visibility = Visibility.Visible;
        }

        private void Procedure_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (lastSelectedButton != null) lastSelectedButton.ClearValue(Button.BackgroundProperty);

            clickedButton.Background = (SolidColorBrush)Application.Current.Resources["LightRose"];
            lastSelectedButton = clickedButton;

            // Запоминаем название процедуры
            selectedProcedure = clickedButton.Content.ToString();

            isItemSelected = true;
            BtnNext.IsEnabled = true;
            BtnNext.Visibility = Visibility.Visible;
        }

        private void Master_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (lastSelectedButton != null) lastSelectedButton.ClearValue(Button.BackgroundProperty);

            clickedButton.Background = (SolidColorBrush)Application.Current.Resources["LightRose"];
            lastSelectedButton = clickedButton;

            selectedMaster = clickedButton.Content.ToString();

            isItemSelected = true;
            BtnNext.IsEnabled = true;
            BtnNext.Visibility = Visibility.Visible;
        }

        private void Time_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (lastSelectedButton != null) lastSelectedButton.ClearValue(Button.BackgroundProperty);

            clickedButton.Background = (SolidColorBrush)Application.Current.Resources["LightRose"];
            lastSelectedButton = clickedButton;

            selectedTime = clickedButton.Content.ToString();

            isItemSelected = true;
            BtnNext.IsEnabled = true;
            BtnNext.Visibility = Visibility.Visible;
        }

        public void ResetView()
        {
            currentStep = 1;
            maxReachedStep = 1;
            selectedProcedure = "";
            selectedMaster = "";
            selectedTime = "";
            isItemSelected = false;
            lastSelectedButton = null;
            UpdateStepUI();
        }
    }
}