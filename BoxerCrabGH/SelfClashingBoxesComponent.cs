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
    public class SelfClashingBoxesComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SelfClashingBoxesComponent()
          : base("Multi Box Clash", "MBX",
              "Finds all the intersections between the bounding boxes of the given geometries",
              "Intersect", "BoxerCrab")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometries", "G", "Geometries from which bounding boxes will be generated", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "P", "Plane to which bounding boxes will be aligned", GH_ParamAccess.item, Plane.WorldXY);
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBoxParameter("Boxes", "B", "All the bounding boxes of input geometry", GH_ParamAccess.list);
            pManager.AddIntegerParameter("First Indices","i1","Index of first box in each clashing pair",GH_ParamAccess.list);
            pManager.AddIntegerParameter("Second Indices", "i2", "Index of second box in each clashing pair", GH_ParamAccess.list);


        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var geoms = new List<IGH_GeometricGoo>();
            DA.GetDataList(0, geoms);
            Plane plane = default;
            DA.GetData(1, ref plane);

            var doXform = !(plane == Plane.WorldXY);
            var xform = Transform.ChangeBasis(plane, Plane.WorldXY);
            var reXform = Transform.ChangeBasis(Plane.WorldXY, plane);

            var boxes = geoms
                .Select(g => doXform ? g.GetBoundingBox(xform) : g.Boundingbox)
                .ToList();
            var boxesSI = boxes.Select(b => b.ToSI()).ToList();
            var pairs = Engine.GetSelfIntersectingPairs(boxesSI, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
            var i1 = pairs.Select(p => p.Index1).ToList();
            var i2 = pairs.Select(p => p.Index2).ToList();
            var boxesGH = 
                boxes
                .Select(b => new GH_Box(b))
                .Select(ghb => doXform ? ghb.Transform(reXform) : ghb)
                .ToList();
            
            DA.SetDataList(0, boxesGH);
            DA.SetDataList(1, i1);
            DA.SetDataList(2, i2);
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
                return Resources.MBX_logo;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2437ebd7-4e76-428a-99ed-0a7b4943c93d"); }
        }
    }
}