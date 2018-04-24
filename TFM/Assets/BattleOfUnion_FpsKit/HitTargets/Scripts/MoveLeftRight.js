var Tar1:GameObject;
var Tar2:GameObject;
var ScaleX:float;
var Speed:float;
enum Mom{
	Tam1=1,
	Tam2=2,	
}
function Start () {
	transform.position.x=Random.Range((Tar1.transform.position.x)+ScaleX,(Tar2.transform.position.x)-ScaleX);
	ScaleX=transform.localScale.x/2;
	TargetMoveSelect=Random.Range(1,3);

}
var TargetMoveSelect=Mom.Tam1;
function Update () {
ScaleX=transform.localScale.x/2;
if(TargetMoveSelect==1){
transform.position.x=Mathf.Lerp(transform.position.x,(Tar1.transform.position.x)+ScaleX,Time.deltaTime*Speed);
}else{
transform.position.x=Mathf.Lerp(transform.position.x,(Tar2.transform.position.x)-ScaleX,Time.deltaTime*Speed);
}
if(transform.position.x<=(Tar1.transform.position.x)+ScaleX+0.2){TargetMoveSelect=2;}
if(transform.position.x>=(Tar2.transform.position.x)-ScaleX-0.2){TargetMoveSelect=1;}



}



