using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace BoxerCrab.GH
{
    public class BoxerCrabGHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Boxer Crab";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "A library for fast clahing/intersection for large groups of bounding boxes";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("05d86e8a-72e5-438a-a25c-acaa5779b682");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Dominic Beer";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
