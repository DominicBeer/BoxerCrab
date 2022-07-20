using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using SpaceIndex;
using System.Linq;
using Grasshopper.Kernel.Types;
// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace BoxerCrab.GH
{
    public class BoxesClashConponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public BoxesClashConponent()
          : base("Box Clash", "BX",
              "Tests whether two bounding boxes intersect",
              "Intersect", "BoxerCrab")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry 1", "G1", "First geometry for box clash.", GH_ParamAccess.item);
            pManager.AddGeometryParameter("Geometry 2", "G2", "Second geometry for box clash.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "Plane to which bounding boxes will be aligned", GH_ParamAccess.item, Plane.WorldXY);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBoxParameter("Box 1", "B1", "Bounding boxes of first input geometry", GH_ParamAccess.item);
            pManager.AddBoxParameter("Box 2", "B2", "Bounding boxes of second input geometry", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Result", "R", "Boolean result, true if boxes clash", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_GeometricGoo goo1 = null;
            DA.GetData(0, ref goo1);

            IGH_GeometricGoo goo2 = null;
            DA.GetData(0, ref goo2);

            Plane plane = default;
            DA.GetData(2, ref plane);

            var doXform = !(plane == Plane.WorldXY);
            var xform = Transform.ChangeBasis(plane, Plane.WorldXY);
            var reXform = Transform.ChangeBasis(Plane.WorldXY, plane);

            var box1 = doXform ? goo1.GetBoundingBox(xform) : goo1.Boundingbox;
            var box2 = doXform ? goo1.GetBoundingBox(xform) : goo1.Boundingbox;

            var result = Engine.BoxesClash(box1.ToSI(), box2.ToSI(), Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);

            var boxGH1 = doXform
                ? new GH_Box(box1).Transform(reXform)
                : new GH_Box(box1);

            var boxGH2 = doXform
                ? new GH_Box(box2).Transform(reXform)
                : new GH_Box(box2);

            DA.SetData(0, boxGH1);
            DA.SetData(1, boxGH2);
            DA.SetData(2, result);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resources.BX_Logo;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("15aca006-d7c9-4eb3-a2b5-af7a53220d74"); }
        }
    }
}