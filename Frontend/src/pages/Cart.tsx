import { useNavigate } from "react-router-dom";
import { mockProducts } from "../temp/mockProducts";

export default function Cart() {
  const navigate = useNavigate();

  // fake cart for now
  const cartItems = [
    { ...mockProducts[0], quantity: 1 },
    { ...mockProducts[2], quantity: 2 },
    { ...mockProducts[4], quantity: 1 },
    { ...mockProducts[4], quantity: 1 },
  ];

  const subtotal = cartItems.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0,
  );

  function removeItem(id: number) {
    console.log("remove product", id);
    // later: setCartItems(...)
  }

  return (
    <div className="max-w-5xl mx-auto">
      <h1 className="text-4xl font-bold text-gray-800 mb-8">Shopping Cart</h1>

      <div className="grid md:grid-cols-3 gap-8">
        {/* Cart Items */}
        <div className="md:col-span-2 space-y-6">
          {cartItems.map((item) => (
            <div
              key={item.id}
              className="bg-white border border-gray-200 rounded-2xl shadow-sm p-5 flex gap-6 items-center"
            >
              <img
                src={item.imageUrl}
                alt={item.name}
                className="w-28 h-28 rounded-xl object-cover"
              />

              <div className="flex-1">
                <h2 className="text-xl font-semibold text-gray-800">
                  {item.name}
                </h2>

                <p className="text-gray-500">Quantity: {item.quantity}</p>

                <p className="text-blue-600 font-bold text-lg mt-2">
                  ${(item.price * item.quantity).toFixed(2)}
                </p>
              </div>

              <button
                onClick={() => removeItem(item.id)}
                className="px-4 py-2 rounded-md border border-red-300 text-red-600 hover:bg-red-50 transition"
              >
                Remove
              </button>
            </div>
          ))}
        </div>

        {/* Summary */}
        <div className="bg-white border border-gray-200 rounded-2xl shadow-sm p-6 h-fit">
          <h2 className="text-2xl font-semibold mb-6 text-gray-800">
            Order Summary
          </h2>

          <div className="flex justify-between mb-4 text-gray-600">
            <span>Subtotal</span>
            <span>${subtotal.toFixed(2)}</span>
          </div>

          <div className="flex justify-between mb-6 text-gray-600">
            <span>Shipping</span>
            <span>Free</span>
          </div>

          <div className="flex justify-between text-xl font-bold mb-6">
            <span>Total</span>
            <span>${subtotal.toFixed(2)}</span>
          </div>

          <button
            onClick={() => navigate("/checkout")}
            className="w-full py-3 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition"
          >
            Proceed to Checkout
          </button>
        </div>
      </div>
    </div>
  );
}
