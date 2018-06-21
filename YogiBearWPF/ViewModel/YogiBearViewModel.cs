using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using YogiBearWPF.Model;

namespace YogiBearWPF.ViewModel
{
    //NézetModell típusa
    public class YogiBearViewModel : ViewModelBase
    {
        private YogiBearModel model;
        private bool paused = true;

        //Properties
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public ObservableCollection<GameField> Fields { get; set; }

        //játékidő
        public String GameTime { get { return TimeSpan.FromSeconds(model.GameTime).ToString(@"mm\:ss"); } }
        //Kosarak száma
        public String Baskets { get { return (model.Baskets).ToString(); } }

        //Events
        public event EventHandler NewGame;
        public event EventHandler ExitGame;
        public event EventHandler<GameOverEventArgs> VM_GameOver;
        
        //Constructor
        public YogiBearViewModel(YogiBearModel m)
        {
            model = m;
            model.GameOver += new EventHandler<GameOverEventArgs>(OnVM_GameOver);
            NewGameCommand = new DelegateCommand(param => { OnNewGame(); SetupTable(); });
            PauseCommand = new DelegateCommand(param => { OnPause(); });
            ExitCommand = new DelegateCommand(param => OnExitGame());
            Fields = new ObservableCollection<GameField>();
            model.Refresh += RefreshTable;
        }

        //Játékmezők frissítése
        private void RefreshGameFields()
        {
            foreach (GameField f in Fields)
                f.Value = model.Map[f.X][f.Y];
        }

        //Játéktábla frissítésének eseménykezelője
        private void RefreshTable(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke((System.Action)delegate
            {
                RefreshGameFields();

                OnPropertyChanged("Fields");
                OnPropertyChanged("GameTime");
                OnPropertyChanged("Baskets");
            });
        }

        //Játéktáblát reprezentáló collection feltöltése
        private void SetupTable()
        {
            Fields.Clear();
            for (Int32 i = 0; i < model.Map.Count; i++)
            {
                for (Int32 j = 0; j < model.Map.Count; j++)
                {
                    Fields.Add(new GameField
                    {
                        Value = model.Map[i][j],
                        X = i,
                        Y = j
                    });
                }
            }
            OnPropertyChanged("Fields");
            OnPropertyChanged("GameTime");
            OnPropertyChanged("Baskets");
        }


        //Event methods
        private void OnPause()
        {
            paused = !paused;

            if(paused)
            {
                model.time.Stop();
                model.patrolling.Stop();
            }else
            {
                model.time.Start();
                model.patrolling.Start();
            }
        }


        // Új játék indításának eseménykiváltása.
        private void OnNewGame()
        {
            if(paused)
            {
                paused = !paused;
            }

            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }

        //Kilépés
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        //Játékos mozgása
        public void MoveBear(int direction)
        {
            if(!paused)
            { 
                switch (direction)
                {
                    case 1:
                        if (model.IsFloor(model.PlayerPos.X - 1, model.PlayerPos.Y)) model.Up();
                        break;
                    case 2:
                        if (model.IsFloor(model.PlayerPos.X + 1, model.PlayerPos.Y)) model.Down();
                        break;
                    case 3:
                        if (model.IsFloor(model.PlayerPos.X, model.PlayerPos.Y + 1)) model.Right();
                        break;
                    case 4:
                        if (model.IsFloor(model.PlayerPos.X, model.PlayerPos.Y - 1)) model.Left();
                        break;
                    default:
                        break;
                }
            }

            RefreshGameFields();
            OnPropertyChanged("Fields");
        }

        //Játék vége eseménykezelője
        private void OnVM_GameOver(object sender, GameOverEventArgs e)
        {
            OnPause();

            if(VM_GameOver != null)
            {
                VM_GameOver(this, new GameOverEventArgs(e.result));
            }
        }

    }

}
