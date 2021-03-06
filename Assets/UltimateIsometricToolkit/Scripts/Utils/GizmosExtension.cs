﻿using System;
using System.Linq;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.UltimateIsometricToolkit.Scripts.Utils {
	public static class GizmosExtension {

		public static void DrawIsoWireCube(Vector3 center, Vector3 size) {
		
			var half = size / 2;
			// draw front
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(-half.x, -half.y, half.z)), Isometric.IsoToScreen(center + new Vector3(half.x, -half.y, half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(-half.x, -half.y, half.z)), Isometric.IsoToScreen(center + new Vector3(-half.x, half.y, half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(half.x, half.y, half.z)), Isometric.IsoToScreen(center + new Vector3(half.x, -half.y, half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(half.x, half.y, half.z)), Isometric.IsoToScreen(center + new Vector3(-half.x, half.y, half.z)));
			// draw back
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(-half.x, -half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(half.x, -half.y, -half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(-half.x, -half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(-half.x, half.y, -half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(half.x, half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(half.x, -half.y, -half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(half.x, half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(-half.x, half.y, -half.z)));
			// draw corners
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(-half.x, -half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(-half.x, -half.y, half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(half.x, -half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(half.x, -half.y, half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(-half.x, half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(-half.x, half.y, half.z)));
			Gizmos.DrawLine(Isometric.IsoToScreen(center + new Vector3(half.x, half.y, -half.z)), Isometric.IsoToScreen(center + new Vector3(half.x, half.y, half.z)));
		
		}

		public static void DrawIsoLine(Vector3 from, Vector3 to) {
			Gizmos.DrawLine(Isometric.IsoToScreen(from), Isometric.IsoToScreen(to));
		}

		public static void DrawIsoArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
			DrawIsoLine(from,to);
			var direction = to - from;
			var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
			var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
			DrawIsoLine(to,to + right * arrowHeadLength);
			DrawIsoLine(to, to + left * arrowHeadLength);
		}


		public static void DrawIsoMesh([NotNull] Mesh mesh, Vector3 position, Vector3 scale) {
			if (mesh == null)
				throw new ArgumentNullException("mesh");
			var verts = mesh.vertices.Select(v => Vector3.Scale(v, scale) + position).ToList();
			var tris = mesh.triangles;
			for (var i = 0; i < tris.Length - 3; i += 3) {
				DrawIsoLine(verts[tris[i]], verts[tris[i + 1]]);
				DrawIsoLine(verts[tris[i + 1]], verts[tris[i + 2]]);
				DrawIsoLine(verts[tris[i]], verts[tris[i + 2]]);
			}
		}

	
	}
}
