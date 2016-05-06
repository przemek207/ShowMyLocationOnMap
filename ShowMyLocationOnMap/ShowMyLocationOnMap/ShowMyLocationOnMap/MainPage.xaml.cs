﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ShowMyLocationOnMap.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location; // Provides the GeoCoordinate class.
using Windows.Devices.Geolocation; //Provides the Geocoordinate class.
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShowMyLocationOnMap
{
    public partial class MainPage : PhoneApplicationPage
    {
        Geolocator myGeolocator = new Geolocator();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
           // ShowMyLocationOnTheMap();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            myGeolocator.MovementThreshold = 50;
            myGeolocator.ReportInterval = 2000;
            myGeolocator.DesiredAccuracy = PositionAccuracy.High;
            myGeolocator.PositionChanged += myGeolocator_PositionChanged;
        }
        private async void ShowMyLocationOnTheMap()
        {
            // Get my current location.
            
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate =
                CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    locationString.Text = "latitude: " + myGeocoordinate.Latitude.ToString() + " \nlongitude " + myGeocoordinate.Longitude.ToString() + " \naltitude: " + myGeocoordinate.Altitude.ToString();


                    // Make my current location the center of the Map.
                    this.mapWithMyLocation.Center = myGeoCoordinate;
                    this.mapWithMyLocation.ZoomLevel = 13;
                    // Create a small circle to mark the current location.
                    Ellipse myCircle = new Ellipse();
                    myCircle.Fill = new SolidColorBrush(Colors.Blue);
                    myCircle.Height = 20;
                    myCircle.Width = 20;
                    myCircle.Opacity = 50;

                    // Create a MapOverlay to contain the circle.
                    MapOverlay myLocationOverlay = new MapOverlay();
                    myLocationOverlay.Content = myCircle;
                    myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                    myLocationOverlay.GeoCoordinate = myGeoCoordinate;

                    // Create a MapLayer to contain the MapOverlay.
                    MapLayer myLocationLayer = new MapLayer();
                    myLocationLayer.Add(myLocationOverlay);

                    // Add the MapLayer to the Map.
                    mapWithMyLocation.Layers.Add(myLocationLayer);
                });

            
        }

        private void get_btn_Click(object sender, RoutedEventArgs e)
        {
            ShowMyLocationOnTheMap();
        }
        void myGeolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            ShowMyLocationOnTheMap();
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}