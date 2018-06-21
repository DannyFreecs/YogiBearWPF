using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YogiBearWPF.Model;
using YogiBearWPF.Persistence;
using YogiBearWPF.View;
using YogiBearWPF.ViewModel;

namespace YogiBearWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Fields

        private YogiBearModel model;
        private YogiBearViewModel viewModel;
        private MainWindow view;

        //Ctor
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //modell létrehozás
            model = new YogiBearModel();

            //viewmodell létrehozás
            viewModel = new YogiBearViewModel(model);
            viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            viewModel.VM_GameOver += new EventHandler<GameOverEventArgs>(ViewModel_GameOver);

            //nézet létrehozása
            view = new MainWindow();
            view.KeyDown += new KeyEventHandler(ViewModel_OnKeyDownHandler);
            view.DataContext = viewModel;
            view.Show();
        }

        // gomb lenyomásának kezelője
        private void ViewModel_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.W: //fel
                    viewModel.MoveBear(1);
                    break;
                case Key.A: //balra
                    viewModel.MoveBear(4);
                    break;
                case Key.S: //le
                    viewModel.MoveBear(2);
                    break;
                case Key.D: //jobbra
                    viewModel.MoveBear(3);
                    break;
                default:
                    break;
            }
        }

        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            model.NewGame();
        }

        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            view.Close(); // ablak bezárása
        }

        private void ViewModel_GameOver(object sender, GameOverEventArgs e)
        {
            if (e.result)
                MessageBox.Show("Gratulálok, győztél!", "Győzelem", MessageBoxButton.OK);
            else
                MessageBox.Show("Sajnos vesztettél :(", "Kudarc", MessageBoxButton.OK);
        }
    }
}
