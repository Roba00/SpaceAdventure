using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTileControl : MonoBehaviour
{
    private Tilemap tilemap;
    private TilemapCollider2D tileCollider;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
        tileCollider = gameObject.GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet" && col.gameObject.GetComponent<BulletInfo>().isMissle)
        {
            //Vector3Int pos = new Vector3Int((int)(col.transform.position.x),
            //(int)(col.transform.position.y), 0);
            //Vector3Int pos2 = tilemap.WorldToCell(pos);
            //Debug.Log(pos);
            //Debug.Log(pos2);
            //Destroy(col.gameObject);
            //tilemap.SetTile(pos2, null);

            //tilemap.Cell
            //Grid tileGrid = tilemap.layoutGrid;
            //Vector3Int cellPos = tileGrid.WorldToCell(col.attachedRigidbody.position);
            // tilePos = tilemap.GetTile(cellPos);
            //tilemap.SetTile(new Vector3Int((int)tilePos.x, (int)tilePos.y, 0), null);
            //Destroy(col.gameObject);

            Vector3 hitPos = Vector3.zero;
            foreach(ContactPoint2D hit in col.contacts)
            {
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPos), null);
            }
            Destroy(col.gameObject);
        }
    }
}
