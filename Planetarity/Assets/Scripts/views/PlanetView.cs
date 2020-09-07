using game.controllers;
using game.logic;
using UnityEngine;
using UnityEngine.UI;

namespace game.views {
    /// <summary>
    /// View for a planet
    /// </summary>
    public class PlanetView : CelectialObjectView {
        public LineRenderer OrbitLine;
        public LineRenderer GravityLine;
        public GunView GunView;
        public SphereCollider Collider;
        public Canvas Hud;
        public HpController HpController;
        public ImageBarFill CooldownBar;
        public Text PlayerNickname;
    }
}