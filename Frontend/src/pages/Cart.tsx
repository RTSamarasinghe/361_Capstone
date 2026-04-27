import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api";

type CartItem = {
  id: number;
  productId: number;
  quantity: number;
};

type Product = {
  id: number;
  name: string;
  price: number;
  imageURL: string;
};

export default function Cart() {
  const navigate = useNavigate();

  const [cartItems, setCartItems] = useState<CartItem[]>([]);
  const [products, setProducts] = useState<Record<number, Product>>({});

  // =========================
  // Load Cart + Products
  // =========================
  useEffect(() => {
    async function fetchCart() {
      try {
        // 1. get user (TEMP: customerId = 1)
        const me = await api.get("/auth/me", {
          params: { customerId: 1 },
        });

        console.log("got user");

        const customerId = me.data.id;
        console.log(me);

        // 2. get cart
        const cartRes = await api.get("/cart", {
          params: { customerId },
        });

        console.log("got cart");
        console.log(cartRes);

        const items: CartItem[] = cartRes.data.items ?? [];
        setCartItems(items);

        // 3. fetch products for each item
        const productMap: Record<number, Product> = {};

        await Promise.all(
          items.map(async (item) => {
            if (!productMap[item.productId]) {
              const res = await api.get(`/products/${item.productId}`);
              productMap[item.productId] = res.data;
            }
          }),
        );

        console.log("got products");

        setProducts(productMap);
      } catch (err) {
        console.error("Failed to load cart", err);
      }
    }

    fetchCart();
  }, []);

  // =========================
  // Subtotal
  // =========================
  const subtotal = cartItems.reduce((sum, item) => {
    const product = products[item.productId];
    if (!product) return sum;
    return sum + product.price * item.quantity;
  }, 0);

  // =========================
  // Remove item
  // =========================
  async function removeItem(itemId: number) {
    try {
      await api.delete(`/cart/items/${itemId}`);
      setCartItems((prev) => prev.filter((i) => i.id !== itemId));
    } catch (err) {
      console.error("Failed to remove item", err);
    }
  }

  // =========================
  // Render
  // =========================
  return (
    <div className="max-w-5xl mx-auto">
      <h1 className="text-4xl font-bold text-gray-800 mb-8">Shopping Cart</h1>

      <div className="grid md:grid-cols-3 gap-8">
        {/* Cart Items */}
        <div className="md:col-span-2 space-y-6">
          {cartItems.map((item) => {
            const product = products[item.productId];
            if (!product) return null;

            return (
              <div
                key={item.id}
                className="bg-white border rounded-2xl shadow-sm p-5 flex gap-6 items-center"
              >
                <img
                  src={`http://localhost:5208/${product.imageURL}`}
                  alt={product.name}
                  className="w-28 h-28 rounded-xl object-cover"
                />

                <div className="flex-1">
                  <h2 className="text-xl font-semibold">{product.name}</h2>

                  <p className="text-gray-500">Quantity: {item.quantity}</p>

                  <p className="text-blue-600 font-bold text-lg mt-2">
                    ${(product.price * item.quantity).toFixed(2)}
                  </p>
                </div>

                <button
                  onClick={() => removeItem(item.id)}
                  className="px-4 py-2 border border-red-300 text-red-600 rounded-md hover:bg-red-50"
                >
                  Remove
                </button>
              </div>
            );
          })}
        </div>

        {/* Summary */}
        <div className="bg-white border rounded-2xl shadow-sm p-6 h-fit">
          <h2 className="text-2xl font-semibold mb-6">Order Summary</h2>

          <div className="flex justify-between mb-4">
            <span>Subtotal</span>
            <span>${subtotal.toFixed(2)}</span>
          </div>

          <div className="flex justify-between mb-6">
            <span>Shipping</span>
            <span>Free</span>
          </div>

          <div className="flex justify-between text-xl font-bold mb-6">
            <span>Total</span>
            <span>${subtotal.toFixed(2)}</span>
          </div>

          <button
            onClick={() => navigate("/checkout")}
            className="w-full py-3 bg-blue-600 text-white rounded-md hover:bg-blue-700"
          >
            Proceed to Checkout
          </button>
        </div>
      </div>
    </div>
  );
}
