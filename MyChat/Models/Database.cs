using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Supabase;
using Supabase.Realtime;
using Client = Supabase.Client;

namespace MyChat.Model
{

    public class Database : INotifyPropertyChanged
    {
        public Database()
        {
            // В конструкторе создаем массив со студентами,
            // в котором будут храниться все строки из таблицы
            Table = new List<users>();

            // Подключаемся к базе данных
            string url = "https://qwspeokhpucujsxjlqvx.supabase.co";
            string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InF3c3Blb2tocHVjdWpzeGpscXZ4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE2NTA1Njc4MzAsImV4cCI6MTk2NjE0MzgzMH0.JitkA0W-Ksk4rMewy3auBapj9yVHJRc_wc3SuvjZlA0";

            Client.InitializeAsync(url, key, new SupabaseOptions
            {
                AutoConnectRealtime = true,
                ShouldInitializeRealtime = true
            });

            // Получаем экземпляр клиента
            Client = Client.Instance;
            // И подписываемся на события изменения в базе данных
            Client.From<users>().On(Client.ChannelEventType.All, UsersChanged);
        }

        // Клиент для обращения к базе данных
        public Client Client { get; }

        // Массив со студентами из базы
        public IEnumerable<users> Table { get; set; }

        // Событие изменения массива для обновления интерфейса
        public event PropertyChangedEventHandler? PropertyChanged;

        // При изменении данных в талице на сервере просто подгружаем данные из нее
        private void UsersChanged(object sender, SocketResponseEventArgs a)
        {
            Debug.WriteLine("changed");
            LoadData();
        }

        // А вот так просходит загрузка данных из талицы
        // на сервере Supabase в массив нашей программы
        public async void LoadData()
        {
            Debug.WriteLine("Data");
            // Берем данные из таблицы и помещаем их в массив
            var data = await Client.From<users>().Get();
            Table = data.Models;

            // Вызов этой функции необходим для автоматического обновления
            // интерфейса программы при изменении данных в массиве со студентами
            OnPropertyChanged(nameof(Table));
        }

        // Реализация интерфейса INotifyPropertyChanged необходима для обновления формы программы
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
