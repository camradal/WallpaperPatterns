﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WallpaperPatterns.WP7.Utilities
{
    public enum Resolutions
    {
        WVGA,
        WXGA,
        HD
    };

    public static class ResolutionHelper
    {
        private static bool IsWvga
        {
            get
            {
                return App.Current.Host.Content.ScaleFactor == 100;
            }
        }

        private static bool IsWxga
        {
            get
            {
                return App.Current.Host.Content.ScaleFactor == 160;
            }
        }

        private static bool IsHD
        {
            get
            {
                return App.Current.Host.Content.ScaleFactor == 150;
            }
        }

        public static Resolutions CurrentResolution
        {
            get
            {
                if (IsWvga) return Resolutions.WVGA;
                else if (IsWxga) return Resolutions.WXGA;
                else if (IsHD) return Resolutions.HD;
                else throw new InvalidOperationException("Unknown resolution");
            }
        }

        public static Size DisplayResolution
        {
            get
            {
                if (Environment.OSVersion.Version.Major < 8)
                    return new Size(480, 800);
                int scaleFactor = (int)GetProperty(Application.Current.Host.Content, "ScaleFactor");
                switch (scaleFactor)
                {
                    case 100:
                        return new Size(480, 800);
                    case 150:
                        return new Size(720, 1280);
                    case 160:
                        return new Size(768, 1280);
                }
                return new Size(480, 800);
            }
        }

        private static object GetProperty(object instance, string name)
        {
            var getMethod = instance.GetType().GetProperty(name).GetGetMethod();
            return getMethod.Invoke(instance, null);
        }
    }
}