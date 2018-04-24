var Healt:float=100;
function ApplyDamage (Damage : float) {
Healt-=Damage;
}
function Update () {
	if(Healt<=0){	
		if(gameObject.tag=="TargetMetal"){
			gameObject.tag="Metal";
				}
		if(gameObject.tag=="TargetWood"){
			gameObject.tag="Wood";
				}
		gameObject.AddComponent.<Rigidbody>();
		Destroy(GetComponent("MoveLeftRight"));
		transform.eulerAngles.x=10;
		gameObject.Destroy(GetComponent("TargetHealt"));
	}
	
	
}