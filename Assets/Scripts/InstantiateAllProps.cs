using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

public class InstantiateAllProps : MonoBehaviour
{
    [SerializeField] private GameObject miniWorldContainer;
    [SerializeField] private RenderTexture renderTexture;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();

        for (int i = 0; i < PropSpriteGenerator.propProperties.Count; i++)
        {
            GameObject propInstance = Instantiate(PropSpriteGenerator.propProperties[i].gameObject, miniWorldContainer.transform);
            propInstance.transform.localPosition = new Vector3(30 + i * -6, -propInstance.GetComponent<Renderer>().bounds.size.y / 2, -16);
            propInstance.transform.rotation = Quaternion.Euler(0, 135, 0);
            propInstance.layer = 7;

            transform.localPosition = new Vector3(30 + i * -6, 0, 0);

            _camera.Render();
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, GraphicsFormatUtility.GetGraphicsFormat(renderTexture.format, false), TextureCreationFlags.None);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            PropSpriteGenerator.propProperties[i].sprite = sprite;
        }
    }
}
