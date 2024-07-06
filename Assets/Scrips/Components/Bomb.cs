using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ComponentBase,ITriggerComponent
{
    private bool bombed;
    
    public float explosionRadius = 5f; // 爆炸的范围
    public float explosionForce = 700f; // 爆炸的力量
    public LayerMask explosionLayer; // 要爆炸的层级

    public void OnTrigger()
    {
        if (!bombed)
        {
            Explode();  
            
            bombed = true;
        }
    }
    
    void Explode()
    {
        // 获取爆炸中心位置
        Vector2 explosionPosition = transform.position;

        // 查找爆炸范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, explosionLayer);

        // 遍历所有碰撞体，并施加爆炸力
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 计算爆炸力的方向和大小
                Vector2 direction = rb.position - explosionPosition;
                float distance = direction.magnitude;
                float explosionEffect = 1 - (distance / explosionRadius);
                Vector2 force = direction.normalized * explosionForce * explosionEffect;

                // 施加爆炸力
                rb.AddForce(force);
            }
        }

        // 输出爆炸信息，方便调试
        Debug.Log($"Explosion at {explosionPosition} with {colliders.Length} objects affected.");
    }

    void OnDrawGizmosSelected()
    {
        // 绘制爆炸范围，方便在编辑器中查看
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}