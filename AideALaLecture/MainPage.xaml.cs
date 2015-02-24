using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AideALaLecture
{
	public sealed partial class MainPage : Page
	{
		private string[] _listeMots = { "elle", "dans", "que", "c'est", "est" };

		private string _motCourant;

		private Random _rnd = new Random();

		public MainPage()
		{
			InitializeComponent();
			ChangeMot();
		}

		private void ChangeMot()
		{
			int i;
			do
			{
				i = _rnd.Next(_listeMots.Length);
			}
			while (_listeMots[i] == _motCourant);
			_motCourant = _listeMots[i];
			MotLabel.Text = _motCourant;
        }
	}
}
