var scene = new THREE.Scene();
var camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);

var directionalLight = new THREE.DirectionalLight(0xFFFFFF);
directionalLight.position.set(1, 1, 1).normalize();
scene.add(directionalLight);

var renderer = new THREE.WebGLRenderer(/*{alpha: true}*/);
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);
renderer.setClearColor(0x263238, 1);

camera.position.y = 2;
camera.position.z = 5;

var effect = new THREE.OutlineEffect(renderer, {
    defaultThickness: 0.005,
    defaultColor: [0, 0, 0],
    defaultAlpha: 1,
    defaultKeepAlive: true // keeps outline material in cache even if material is removed from scene
});

var shininess = 0.1;
var specular = 0.1;

var diffuseColor = new THREE.Color(0xc86464);
var specularShininess = Math.pow(2, shininess * 10);
var specularColor = new THREE.Color(specular * 0.2, specular * 0.2, specular * 0.2);
var material = new THREE.MeshToonMaterial({
    color: diffuseColor,
    specular: specularColor,
    reflectivity: specular,
    shininess: specularShininess,
});
var geometry = new THREE.SphereGeometry(1, 32, 16);
var cube = new THREE.Mesh(geometry, material);
scene.add(cube);

window.addEventListener('resize', onWindowResize, false);
function onWindowResize() {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
}

var animate = function () {
    requestAnimationFrame(animate);

    renderer.render(scene, camera);
    effect.render(scene, camera);
};

animate();