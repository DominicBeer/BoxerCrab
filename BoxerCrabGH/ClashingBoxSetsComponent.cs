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
    public class ClashingBoxSetsConponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ClashingBoxSetsConponent()
          : base("Box Sets Clash", "BBX",
              "Finds the bounding box intersections between the two sets of given geometries",
              "Intersect", "BoxerCrab")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Set 1", "G1", "First set of geometries", GH_ParamAccess.list);
            pManager.AddGeometryParameter("Set 2", "G2", "Second set of geometries", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "P", "Plane to which bounding boxes will be aligned", GH_ParamAccess.item, Plane.WorldXY);
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBoxParameter("Boxes 1", "B1", "All the bounding boxes of input geometry in set 1", GH_ParamAccess.list);
            pManager.AddBoxParameter("Boxes 2", "B2", "All the bounding boxes of input geometry in set 2", GH_ParamAccess.list);
            pManager.AddIntegerParameter("First Index", "i1", "Index of box from set 1 in clashing pair", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Second Index", "i2", "Index of box from set 2 in clashing pair", GH_ParamAccess.list);



        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var geoms1 = new List<IGH_GeometricGoo>();
            DA.GetDataList(0, geoms1);
            var geoms2 = new List<IGH_GeometricGoo>();
            DA.GetDataList(0, geoms2);
            Plane plane = default;
            DA.GetData(1, ref plane);

            var doXform = !(plane == Plane.WorldXY);
            var xform = Transform.ChangeBasis(plane, Plane.WorldXY);
            var reXform = Transform.ChangeBasis(Plane.WorldXY, plane);

            var boxes1 = geoms1
                .Select(g => doXform ? g.GetBoundingBox(xform) : g.Boundingbox)
                .ToList();

            var boxes2 = geoms2
                .Select(g => doXform ? g.GetBoundingBox(xform) : g.Boundingbox)
                .ToList();




            var boxesSI1 = boxes1.Select(b => b.ToSI()).ToList();
            var boxesSI2 = boxes1.Select(b => b.ToSI()).ToList();
            var pairs = Engine.GetIntersectingPairs(boxesSI1, boxesSI2, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
            var i1 = pairs.Select(p => p.Index1).ToList();
            var i2 = pairs.Select(p => p.Index2).ToList();
            var boxesGH1 =
                boxes1
                .Select(b => new GH_Box(b))
                .Select(ghb => doXform ? ghb.Transform(reXform) : ghb)
                .ToList();
            var boxesGH2 =
                boxes2
                .Select(b => new GH_Box(b))
                .Select(ghb => doXform ? ghb.Transform(reXform) : ghb)
                .ToList();

            DA.SetDataList(0, boxesGH1);
            DA.SetDataList(1, boxesGH2);
            DA.SetDataList(2, i1);
            DA.SetDataList(3, i2);
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
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("315996d5-a987-48be-b6dd-d6df6bc58be3"); }
        }
    }
}