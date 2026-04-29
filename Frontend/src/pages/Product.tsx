import { useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import api from "../api";
import type { Product as ProductType } from "../types/Product";

export default function Product() {
  const { id } = useParams();

  const [product, setProduct] = useState<ProductType | null>(null);
  const [quantity, setQuantity] = useState(1);
  const [cartId, setCartId] = useState<number | null>(null);

  const [loading, setLoading] = useState(true);
  const [adding, setAdding] = useState(false);

  // =========================
  // Fetch Product
  // =========================
  useEffect(() => {
    async function fetchProduct() {
      try {
        const res = await api.get(`/products/${id}`);
        setProduct(res.data);
      } catch (err) {
        console.error("Failed to fetch product", err);
      } finally {
        setLoading(false);
      }
    }

    if (id) fetchProduct();
  }, [id]);

  // =========================
  // Fetch Cart ID (TEMP: using customerId=1)
  // =========================
  useEffect(() => {
    async function fetchCart() {
      try {
        const res = await api.get("/auth/me", {
          params: { customerId: 8 }, // ⚠️ TEMP (your backend requires this)
        });

        setCartId(res.data.cartId);
      } catch (err) {
        console.error("Failed to fetch cartId", err);
      }
    }

    fetchCart();
  }, []);

  // =========================
  // Add to Cart
  // =========================
  async function handleAddToCart() {
    if (!product || !cartId) return;

    try {
      setAdding(true);

      await api.post("/cart/items", {
        cartId: cartId,
        productId: product.id,
        quantity: Math.max(1, quantity),
      });

      console.log("Added to cart");

      // UX improvement
      setQuantity(1);
    } catch (err: any) {
      console.error("Failed to add to cart", err.response?.data);
    } finally {
      setAdding(false);
    }
  }

  // =========================
  // UI States
  // =========================
  if (loading) {
    return <div className="text-gray-600">Loading product...</div>;
  }

  if (!product) {
    return <div className="text-gray-600">Product not found.</div>;
  }

  // =========================
  // Render
  // =========================
  return (
    <div className="max-w-6xl mx-auto grid md:grid-cols-2 gap-10">
      {/* Image */}
      <div className="bg-white rounded-2xl shadow-md border border-gray-200 overflow-hidden">
        <img
          src={`http://localhost:5208/${product.imageURL}`}
          alt={product.name}
          className="w-full h-full object-cover"
        />
      </div>

      {/* Info */}
      <div className="flex flex-col">
        <h1 className="text-3xl font-bold text-gray-800 mb-3">
          {product.name}
        </h1>

        <p className="text-gray-600 mb-4">{product.description}</p>

        <p className="text-2xl font-bold text-blue-600 mb-6">
          ${product.price.toFixed(2)}
        </p>

        {/* Extra Info */}
        <div className="text-sm text-gray-500 mb-6 space-y-1">
          <p>Manufacturer: {product.manufacturuer}</p>
          <p>SKU: {product.sku}</p>
          <p>Stock: {product.stockQuantity}</p>
          <p>Rating: ⭐ {product.rating}</p>
        </div>

        {/* Quantity + Add */}
        <div className="flex items-center gap-4 mb-6">
          <input
            type="number"
            min={1}
            max={product.stockQuantity}
            value={quantity}
            onChange={(e) =>
              setQuantity(
                Math.max(
                  1,
                  Math.min(product.stockQuantity, Number(e.target.value)),
                ),
              )
            }
            className="w-20 px-3 py-2 border border-gray-300 rounded-md"
          />

          <button
            onClick={handleAddToCart}
            disabled={adding || !cartId}
            className="px-6 py-2 rounded-md bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50"
          >
            {adding ? "Adding..." : "Add to Cart"}
          </button>
        </div>
      </div>
    </div>
  );
}
