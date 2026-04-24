import { Navigate, Outlet, useLocation } from "react-router-dom";

type Props = {
  token: string | null;
};

export default function ProtectedRoute({ token }: Props) {
  const location = useLocation();

  if (!token) {
    return (
      <Navigate
        to="/login"
        state={{
          from: location,
          message: "You must be logged in to access that page.",
        }}
        replace
      />
    );
  }

  return <Outlet />;
}
