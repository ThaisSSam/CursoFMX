import type { ButtonHTMLAttributes, PointerEvent } from "react";

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  children: React.ReactNode;
  disabled?: boolean;
  isLoading?: boolean;
  isLoadingText?: string;
  onClick?: (event: PointerEvent<HTMLButtonElement>) => void;
};

function Button({
  children,
  disabled = false,
  isLoading = false,
  isLoadingText = "Carregando...",
  onClick,
  ...props
}: ButtonProps) {
  return (
    <button
      className="custom-button"
      onClick={onClick}
      disabled={disabled || isLoading}
      {...props}
    >
      {isLoading ? isLoadingText : children}
    </button>
  );
}

export default Button;