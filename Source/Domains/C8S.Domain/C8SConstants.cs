﻿namespace C8S.Domain;

public static class C8SConstants
{
    public static class BlobContainers
    {
        public const string Images = "images";
        public const string Labels = "labels";
        public const string QRCodes = "qrcodes";
        public const string ApplicationsUnread = "c8s-applications";
        public const string ApplicationsProcessed = "c8s-applications-processed";
        public const string ApplicationsError = "c8s-applications-error";
    }

    public static class Defaults
    {
        public const string DefaultAdminEmails = "development@bedtimemath.org";
    }

    public static class Entities
    {
        public const string Note = "Note";
        public const string Person = "Person";
        public const string WPUser = "WPUser";
    }

    public static class Icons
    {
        public const string Add = "plus";
        public const string Clear = "broom-wide";
        public const string Close = "circle-xmark";
        public const string Contact = "user-hair-long";
        public const string Delete = "trash";
        public const string Edit = "pencil";
        public const string Fulco = "boxes-packing";
        public const string Home = "house";
        public const string Link = "link";
        public const string Order = "cart-shopping";
        public const string Organization = "buildings";
        public const string Refresh = "arrow-rotate-right";
        public const string Request = "clipboard-list";
        public const string Save = "floppy-disk";
        public const string Site = "school";
        public const string Offer = "barcode";
        public const string Settings = "gear";
        public const string Ticket = "ticket";
        public const string Unlink = "link-slash";
        public const string WordPress = "wordpress";
    }

    public static class LogTables
    {
        public const string FunctionsLog = "FunctionsLog";
        public const string AdminLog = "AdminLog";
    }
}