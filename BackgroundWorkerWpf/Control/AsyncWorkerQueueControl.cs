using ReactiveAsyncWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

//using DynamicData;

using System.Collections;
using UtilityWpf;
using System.Windows.Markup;
using UtilityWpf.View;
using Reactive.Bindings;
using System.Collections.ObjectModel;

namespace ReactiveAsyncWorkerWpf
{

    public abstract class AsyncWorkerQueueControl<R, T> : Control where R : IWorkerItem<T>
    {

        public static readonly DependencyProperty NewItemProperty = DependencyProperty.Register("NewItem", typeof(object), typeof(AsyncWorkerQueueControl<R, T>), new PropertyMetadata(null, NewItemChanged));

        public static readonly DependencyProperty ItemsSinkProperty = DependencyProperty.Register("ItemsSink", typeof(ObservableCollection<object>), typeof(AsyncWorkerQueueControl<R, T>));

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(AsyncWorkerQueueControl<R, T>));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<object>), typeof(AsyncWorkerQueueControl<R, T>));

        public ObservableCollection<object> ItemsSource
        {
            get { return (ObservableCollection<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object NewItem
        {
            get { return (object)GetValue(NewItemProperty); }
            set { SetValue(NewItemProperty, value); }

        }

        public ObservableCollection<object> ItemsSink
        {
            get { return (ObservableCollection<object>)GetValue(ItemsSinkProperty); }
            set { SetValue(ItemsSinkProperty, value); }
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }



        private static void NewItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AsyncWorkerQueueControl<R, T>).NewItemSubject.OnNext(e.NewValue);
        }


        protected ISubject<object> NewItemSubject = new Subject<object>();
        protected List<IWorkerItem<object>> sink = new List<IWorkerItem<object>>();

        static AsyncWorkerQueueControl()
        {
            //ListBoxEx.SelectedItemProperty.OverrideMetadata(typeof(AsyncWorkerQueueControl<R,T>, new FrameworkPropertyMetadata(null, SelectedItemChanged));
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(AsyncWorkerQueueControl<R, T>), new FrameworkPropertyMetadata(typeof(AsyncWorkerQueueControl<R, T>)));

        }



        public AsyncWorkerQueueControl(UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, R>> workerqueue = null)
        {

            //Uri resourceLocater = new Uri("/ReactiveAsyncWorkerWpf;component/Themes/QueueWorkerControl.xaml", System.UriKind.Relative);
            //ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            //Style = resourceDictionary["BackgroundWorkerQueueControlStyle"] as Style;

            var sets = Observable.FromEventPattern<RoutedEventHandler, EventArgs>(_ => this.Loaded += _, _ => this.Loaded -= _)
                //.StartWith(workerqueue).Where(_ => _ != null)
                .Take(1)
                //.Select(_ => )
                .Subscribe(_x =>
                {

                    var _ = workerqueue ?? NewItemsInitialise(NewItemSubject);
                    ItemsSource = new ObservableCollection<object>();
                    ItemsSink = new ObservableCollection<object>();
                    var dis = _.Resource.Subscribe(asv =>
                    {
                        switch (asv.Key)
                        {
                            case (DynamicData.ChangeReason.Add):
                                this.Dispatcher.InvokeAsync(() =>
                                {
                                    ItemsSource.Add(asv.Value);
                                    Count = ItemsSource.Count;
                                }, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
                                break;
                            case (DynamicData.ChangeReason.Remove):
                                this.Dispatcher.InvokeAsync(() =>
                                {
                                    ItemsSource.Remove(asv.Value);
                                    ItemsSink.Add(asv.Value);
                                    Count = ItemsSource.Count;
                                }, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));

                                break;

                        }

                    });


                    /* this.Dispatcher.InvokeAsync(() => { ItemsSource = additions; }, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));*/
                });
        }

        protected abstract UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, R>> NewItemsInitialise(IObservable<object> newitems);
        //{
        //    return new AsyncWorkerQueue<object>(NewItemSubject);

        //}

    }
}
