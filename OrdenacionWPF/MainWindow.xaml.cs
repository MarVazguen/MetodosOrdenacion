using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OrdenacionWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        #region  animacion
        DispatcherTimer timer;
        double panelWidth;
        bool hidden;
        #endregion
        //Color Tiker
        int[] valors;
        double midaUnitaria;
        Rectangle[] rectangles;
        private bool inici = true;
        private bool intercaviActivat = false;
        public MainWindow()
        {
            InitializeComponent();

           // GenerarGrafica();
            //codi de animacio
            ContingutAnimacio();
             
        }
        public void GenerarGrafica()
        {
            cnvTauler.Children.Clear(); //eliminem tots els fils

            rectangles = new Rectangle[valors.Length];
            midaUnitaria = cnvTauler.Height / valors.Length;

            for (int i = 0; i < rectangles.Length; i++)
            {
                rectangles[i] = new Rectangle();
                if (valors[i] != i) 
                { 
                  rectangles[i].Fill = new SolidColorBrush(ColorPkrIncorrecte.SelectedColor.Value);//  incorecte;
                }
                else if(valors[i] == i)
                {
                    rectangles[i].Fill = new SolidColorBrush(ColorPkrCorrecte.SelectedColor.Value);  //corecte
                }
                //Grosor de rectangle
                rectangles[i].StrokeThickness = (int) intUpDwnGruixMarc.Value;
                //color borde
                rectangles[i].Stroke = new SolidColorBrush(Colors.Black);
                //radi diametre
                rectangles[i].RadiusX =  (int)intUpDwnRadiColumn.Value;
                rectangles[i].RadiusY =  (int)intUpDwnRadiColumn.Value;
                
                cnvTauler.Children.Add(rectangles[i]);
               

                AssignaAlcadaRectangel(i);
            }

        } 
        #region Grafica
        private void AssignaAlcadaRectangel(int nRectangles)
        { 
            //psocionar horizontal

            if(object.Equals( cmbTipoFigura.Text, TipusFigura.Punts.ToString()))
            { 
                rectangles[nRectangles].Width = 10;//midaUnitaria;
                Canvas.SetLeft(rectangles[nRectangles], midaUnitaria * nRectangles);
                rectangles[nRectangles].Height = 10;//midaUnitaria * (valors[nRectangles]);
            }
            else if(object.Equals(cmbTipoFigura.Text, TipusFigura.Barres.ToString()))
            {
                rectangles[nRectangles].Width = midaUnitaria;
                Canvas.SetLeft(rectangles[nRectangles], midaUnitaria * nRectangles);
                rectangles[nRectangles].Height = midaUnitaria * (valors[nRectangles]);
            }
            else
            {
                //imprimir error
            }
            Canvas.SetTop(rectangles[nRectangles], midaUnitaria * (valors.Length - valors[nRectangles]));
           
        }

        #region Threads
        Thread thread;
        private void Espera(double milliseconds)
        {
            var frame = new DispatcherFrame();
            thread = new Thread((ThreadStart)(() =>
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(milliseconds));
                frame.Continue = false;
            }));
            thread.Start();
            Dispatcher.PushFrame(frame);
        }
        static Action action;
        public static void DoEvents()
        {
            action = new Action(delegate { });
            Application.Current?.Dispatcher?.Invoke(
               System.Windows.Threading.DispatcherPriority.Background,
               action);
        }
        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Dispatcher.InvokeShutdown();
            thread?.Abort();
            base.OnClosed(e);
        }
        #endregion
        #endregion

        #region Regio Animaico UI: Barra menu
        private void ContingutAnimacio()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;

            panelWidth = sidePanel.Width;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                sidePanel.Width += 1;
                if (sidePanel.Width >= panelWidth)
                {
                    timer.Stop();
                    hidden = false;
                    listBoxMenu.Visibility = Visibility.Visible;
                    tbConfiguracio.Visibility = Visibility.Visible;
                }
            }
            else
            {
                sidePanel.Width -= 1;
                if (sidePanel.Width <= 25)
                {
                    timer.Stop();
                    hidden = true;
                    listBoxMenu.Visibility = Visibility.Hidden;
                    tbConfiguracio.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void PanelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnThisClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //animatin Menu
        private void tbTitle_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion

        #region Acciones de Botones Conf.Generals
        private void intUpDwnGruixMarc_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
             
        }
        #region CHeckbox
        private void cbInvertit_Checked(object sender, RoutedEventArgs e)
        {
            cbAliatori.IsChecked = false;
        }

        private void cbAliatori_Checked(object sender, RoutedEventArgs e)
        {
            cbInvertit.IsChecked = false;
        }
        #endregion
        #region COLORS: ColorPicker
        private void ColorPkrCorrecte_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!inici)
                GenerarGrafica();
        }

        private void ColorPkrIncorrecte_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!inici)
                GenerarGrafica();
           
        }

        private void ColorPkrIntercanviar_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!inici)
                GenerarGrafica();
        }
        #endregion
        #endregion

        #region Algorisme
        private void Quick_Sort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);
                if (pivot > 1)
                {
                    Quick_Sort(arr, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    Quick_Sort(arr, pivot + 1, right);
                }
            }

         
        }
        private int Partition(int[] arr, int left, int right)
        {
            int aux;
            int pivot = arr[left];
            while (true)
            {
                while (arr[left] < pivot)
                {
                    left++;
                }
                while (arr[right] > pivot)
                {
                    right--;
                }
                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    aux = arr[left];
                    arr[left] = arr[right];
                    arr[right] = aux;

                    IntercanviarRectangles(rectangles[left], rectangles[right], left, right);

                    // ComprovaTaulaDesordenada();
                    DoEvents();
                }
                else
                {
                    return right;
                }
            }
        }

        private void IntercanviarRectangles(Rectangle rectangle1, Rectangle rectangle2, int i, int j)
        {            rectangle1.Fill = new SolidColorBrush(ColorPkrIntercanviar.SelectedColor.Value);   
            Canvas.SetZIndex(rectangle1, 1);
            rectangle2.Fill = new SolidColorBrush(ColorPkrIntercanviar.SelectedColor.Value);
            Canvas.SetZIndex(rectangle2, 1);
            //Fem la animacio 
            AnimcacionBarra(rectangle1, rectangle2);
             

            #region Fem canvi Posicio

            double leftAux;
            double bottomAux;

            int indexRect1 = Array.IndexOf(rectangles, rectangle1);
            int indexRect2 = Array.IndexOf(rectangles, rectangle2);

            Rectangle rectAux = rectangle1;
            rectangles[indexRect1] = rectangle2;
            rectangles[indexRect2] = rectAux;


            DoEvents();

            #region No utilitzat
            /*
            //topAux = Canvas.GetTop(rectangle1);
            leftAux = Canvas.GetLeft(rectangle1);
            bottomAux = Canvas.GetBottom(rectangle1);

            //rectangle1.ClearValue(Canvas.TopProperty);
            //  rectangle1.ClearValue(Canvas.TopProperty);
            // rectangle1.ClearValue(Canvas.TopProperty);

            // Canvas.SetTop(rectangle1, Canvas.GetTop(rectangle2));
            Canvas.SetLeft(rectangle1, Canvas.GetLeft(rectangle2));
            Canvas.SetBottom(rectangle1, Canvas.GetBottom(rectangle2));

            //  rectangle2.ClearValue(Canvas.TopProperty);
            // rectangle2.ClearValue(Canvas.TopProperty);
            // rectangle2.ClearValue(Canvas.TopProperty);

            //  Canvas.SetTop(rectangle2, topAux);
            */
            #endregion
            leftAux = Canvas.GetLeft(rectangle1);
            bottomAux = Canvas.GetBottom(rectangle1);
            

            Canvas.SetLeft(rectangle1, Canvas.GetLeft(rectangle2));
            Canvas.SetBottom(rectangle1, Canvas.GetBottom(rectangle2));

            Canvas.SetLeft(rectangle2, leftAux);
            Canvas.SetRight(rectangle2, bottomAux);

            Canvas.SetZIndex(rectangle1, 0);
            Canvas.SetZIndex(rectangle2, 0);

            DoEvents();
            int temps = (int)intUpDwnTmpsPausa.Value * 1000;
            Thread.Sleep(temps);
            

            if (valors[i] != i)
            {
                rectangles[i].Fill = new SolidColorBrush(ColorPkrIncorrecte.SelectedColor.Value);//  incorecte;
            }
            else
            {
                rectangles[i].Fill = new SolidColorBrush(ColorPkrCorrecte.SelectedColor.Value);  //corecte
            }
            if (valors[j] != j)
            {
                rectangles[j].Fill = new SolidColorBrush(ColorPkrIncorrecte.SelectedColor.Value);//  incorecte;
            }
            else
            {
                rectangles[j].Fill = new SolidColorBrush(ColorPkrCorrecte.SelectedColor.Value);  //corecte
            }
            #endregion
        }
        #endregion

        #region GenerarArray
        public static int[] GenerarOrdenatCreixent(int mida) 
        {
            int[] arr = new int[mida];

            for(int i = 0; i < mida; i++)
            {
                arr[i] = i;
            }
            return arr;
        }
        public static int[]GemerarOrdenatDecreixent(int mida) 
        {
            int[] arr = new int[mida];
            for (int i = 0; i < mida; i++)
            {
                arr[i] = mida-i;
            }
            return arr;
        }

        public static int[]GenerarAtzar(int mida)
        {
            int[] arr = new int[mida];
            int number = 0;
            Random rm = new Random();

            for (int i = 0; i < mida; i++)
            {
                number = rm.Next(1, mida + 1);
                while (arr.Contains(number) == true & number != 0)
                    number = rm.Next(1, mida + 1);
                arr[i] = number;
            }
            return arr;
        }
        #endregion

        #region Generar animacion Grafica

        public void AnimcacionBarra(Rectangle dreta,Rectangle esquerra)
        {

            int indexDreta = Array.IndexOf(rectangles, dreta);
            int indexEsquerra = Array.IndexOf(rectangles, esquerra);
            DoubleAnimation animacioesquerra = new DoubleAnimation();
            DoubleAnimation animaciodreta = new DoubleAnimation();

            if (cmbTipoFigura.Text == TipusFigura.Barres.ToString())
            {

                if (cbInvertit.IsChecked == true)
                {
                    animacioesquerra.To = esquerra.Height - dreta.ActualWidth;
                    animacioesquerra.From = dreta.Height - 130;

                    animaciodreta.To = dreta.Height - esquerra.ActualWidth;
                    animaciodreta.From = esquerra.Height;


                    animacioesquerra.Duration = TimeSpan.FromSeconds(1);
                    animaciodreta.Duration = TimeSpan.FromSeconds(1);

                    esquerra.BeginAnimation(Canvas.LeftProperty, animacioesquerra);
                    dreta.BeginAnimation(Canvas.LeftProperty, animaciodreta);
                }
                else if (cbAliatori.IsChecked == true)
                {
                    #region No utilitzat
                    /*
                    animacioesquerra.To = esquerra.Height ;
                    animacioesquerra.From = dreta.Height;
                    animaciodreta.To = dreta.Height -200;
                    animaciodreta.From = esquerra.Height;


                     animacioesquerra.Duration = TimeSpan.FromSeconds(1);
                    animaciodreta.Duration = TimeSpan.FromSeconds(1);

                    esquerra.BeginAnimation(Canvas.LeftProperty, animacioesquerra);
                    dreta.BeginAnimation(Canvas.LeftProperty, animaciodreta);

                    animaciodreta.EasingFunction = new CubicEase();
                    animacioesquerra.EasingFunction = new CubicEase();

                    esquerra.BeginAnimation(Rectangle.HeightProperty, animacioesquerra);
                    dreta.BeginAnimation(Rectangle.HeightProperty, animaciodreta);
                    */
                    #endregion

                    animacioesquerra.To = indexDreta * (dreta.Width);
                    animaciodreta.To = indexEsquerra * esquerra.Width;

                    animacioesquerra.Duration = TimeSpan.FromSeconds(1);
                    animaciodreta.Duration = TimeSpan.FromSeconds(1);

                    esquerra.BeginAnimation(Canvas.LeftProperty, animacioesquerra);
                    dreta.BeginAnimation(Canvas.LeftProperty, animaciodreta);

                }



            }
            else if (cmbTipoFigura.Text == TipusFigura.Punts.ToString())
            {


                animacioesquerra.To = valors[indexDreta] -1;
                animaciodreta.To = cnvTauler.Height - cnvTauler.Height/ valors[indexEsquerra];
                
                animacioesquerra.FillBehavior = FillBehavior.Stop;
                animaciodreta.FillBehavior = FillBehavior.Stop;

                animacioesquerra.Duration = TimeSpan.FromSeconds(1);
                animaciodreta.Duration = TimeSpan.FromSeconds(1);

                animaciodreta.EasingFunction = new CubicEase();
                animacioesquerra.EasingFunction = new CubicEase();

                esquerra.BeginAnimation(Canvas.TopProperty, animacioesquerra);
                dreta.BeginAnimation(Canvas.TopProperty, animaciodreta);
                 
            }
            else
            {
                //error misatge
            }

            //espera
            
            int temps= (int)intUpDwnTmpsPausa.Value;
            DispatcherFrame frm = new DispatcherFrame();
            new Thread((ThreadStart)(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(temps));
                frm.Continue = false;
            })).Start();
            Dispatcher.PushFrame(frm);
            
        }
       

        public void AnimacioCorrecteFinal()
        {
           // cnvTauler.Children.Clear(); //eliminem tots els fils

            rectangles = new Rectangle[valors.Length];
            midaUnitaria = cnvTauler.Height / valors.Length;

            for (int i = 0; i < rectangles.Length; i++)
            {
                rectangles[i] = new Rectangle();
                
                rectangles[i].Fill = new SolidColorBrush(ColorPkrIncorrecte.SelectedColor.Value);//  correcte;
                
                     
                //Grosor de rectangle
                rectangles[i].StrokeThickness = (int)intUpDwnGruixMarc.Value;
                //color borde
                rectangles[i].Stroke = new SolidColorBrush(Colors.Black);
                //radi diametre
                rectangles[i].RadiusX = (int)intUpDwnRadiColumn.Value;
                rectangles[i].RadiusY = (int)intUpDwnRadiColumn.Value;

                cnvTauler.Children.Add(rectangles[i]);

                int temps =300;
                Thread.Sleep(temps);
                
                DoEvents();

                AssignaAlcadaRectangel(i);
            }
          
        }

        #endregion


            #region Tractament de algorisme (Generar, Ordenar)
        private enum TipoOrdenacio { invertit, aleatori };
        private enum TipusFigura { Barres, Punts }
        private enum TiposAlgorismeOrdenacio { Bombolla, Seleccio, Quicksort }

        //Button Generar
        private void btnGenerarGrafica_Click(object sender, RoutedEventArgs e)
        {

            if (cbAliatori.IsChecked == false && cbInvertit.IsChecked == false) { cbInvertit.IsChecked = true; }
            
            #region Recollida dades
            int midaArray =Convert.ToInt32( intUpDwnNElements.Value);

            #endregion

            #region Asignar els valors
            if (midaArray >= 10)
            {
                if(cbInvertit.IsChecked==true)
                {
                    valors = GemerarOrdenatDecreixent(midaArray);
                }
                else if(cbAliatori.IsChecked==true)
                {
                    valors = GenerarAtzar(midaArray);
                }
                else
                {

                }
            }
            else { }//imprimir error
            #endregion


            //Generar grafica
            GenerarGrafica();



        }

        //Button Ordenar
        private void btnOrdenarGrafica_Click(object sender, RoutedEventArgs e)
        {
            if (Object.Equals(cmbAlgorismeOrdenacio.Text, TiposAlgorismeOrdenacio.Quicksort.ToString()))
            {
                Quick_Sort(valors,0,valors.Length-1);
            }
            else if (Object.Equals(cmbAlgorismeOrdenacio.Text, TiposAlgorismeOrdenacio.Bombolla.ToString())) { }
            else if (Object.Equals(cmbAlgorismeOrdenacio.Text, TiposAlgorismeOrdenacio.Seleccio.ToString())) { }
            else { }//imprimir error

            //pasem la color correcte
             AnimacioCorrecteFinal();
        }
        #endregion

        private void chboxIntercanvis_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            intercaviActivat = chboxIntercanvis.IsChecked==true;
        }

      
    }


}
