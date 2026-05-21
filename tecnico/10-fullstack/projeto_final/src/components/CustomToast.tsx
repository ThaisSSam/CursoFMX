import React, { useEffect, useRef } from 'react';
import { Icon, X } from 'lucide-react';
// import { Icon, type IconName } from '../icons';

interface CustomToastProps {
  title: string;
  message: string;
  onClose: () => void;
  type?: 'success' | 'error' | 'warning' | 'info';
  duration?: number;
  persistent?: boolean;
}

const CustomToast: React.FC<CustomToastProps> = React.memo(
  ({ title, message, onClose, type = 'success', duration = 10000, persistent = false }) => {
    const onCloseRef = useRef(onClose);
    // const timerRef = useRef<NodeJS.Timeout | null>(null);

    const toastDuration = duration ?? 10000;

    useEffect(() => {
      onCloseRef.current = onClose;
    }, [onClose]);

    // useEffect(() => {
    //   if (timerRef.current) {
    //     clearTimeout(timerRef.current);
    //     timerRef.current = null;
    //   }

    //   if (!persistent) {
    //     timerRef.current = setTimeout(() => {
    //       onCloseRef.current();
    //     }, toastDuration);
    //   }

    //   return () => {
    //     if (timerRef.current) {
    //       clearTimeout(timerRef.current);
    //       timerRef.current = null;
    //     }
    //   };
    // }, [toastDuration, persistent]);
    const getTypeStyles = () => {
      switch (type) {
        case 'success':
          return {
            container: 'bg-accent-green-surface border-accent-green-main',
            icon: 'bg-accent-green-main',
            iconName: 'CheckWhite' as IconName,
          };
        case 'error':
          return {
            container: 'bg-accent-red-surface border-accent-red-main',
            icon: 'bg-accent-red-main',
            iconName: 'ErrorCircle' as IconName,
          };
        case 'warning':
          return {
            container: 'bg-accent-orange-surface border-accent-orange-main',
            icon: 'bg-accent-orange-main',
            iconName: 'WarningPath' as IconName,
          };
        case 'info':
          return {
            container: 'bg-accent-blue-surface border-accent-blue-main',
            icon: 'bg-accent-blue-main',
            iconName: 'Info' as IconName,
          };
        default:
          return {
            container: 'bg-accent-green-surface border-accent-green-main',
            icon: 'bg-accent-green-main',
            iconName: 'CheckWhite' as IconName,
          };
      }
    };

    const styles = getTypeStyles();

    const processMessage = () => {
      if (!message) return { mainMessage: '', errors: [] };

      const messageStr = typeof message === 'string' ? message : String(message);
      const parts = messageStr.split(/\n\s*\n/);

      if (parts.length > 1) {
        const mainMessage = parts[0].trim();
        const errorsText = parts.slice(1).join('\n');
        const errors = errorsText.split('\n').filter((line) => line.trim() !== '');

        return {
          mainMessage,
          errors,
        };
      }

      const lines = messageStr.split('\n').filter((line) => line.trim() !== '');

      if (lines.length > 1) {
        return {
          mainMessage: lines[0],
          errors: lines.slice(1),
        };
      }

      return {
        mainMessage: messageStr,
        errors: [],
      };
    };

    const { mainMessage, errors } = processMessage();

    return (
      <div
        data-property-1={
          type === 'success'
            ? 'Success'
            : type === 'error'
              ? 'Error'
              : type === 'warning'
                ? 'Warning'
                : 'Info'
        }
        className={`w-[600px] p-3 border ${styles.container} rounded-lg inline-flex justify-start items-start gap-4`}
        style={{ boxShadow: '0 4px 12px 0 rgba(0, 0, 0, 0.06), 0 24px 60px 0 rgba(0, 0, 0, 0.12)' }}
        onClick={(e) => e.stopPropagation()}
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className="flex-1 self-stretch flex justify-start items-start gap-3">
          <div className="pt-1 flex justify-start items-start overflow-hidden">
            <div
              data-danger-icon={type === 'error' ? 'true' : undefined}
              data-success-icon={type === 'success' ? 'true' : undefined}
              data-warning-icon={type === 'warning' ? 'true' : undefined}
              data-info-icon={type === 'info' ? 'true' : undefined}
              className={`w-4.5 h-4.5 p-0.5 ${styles.icon} rounded-3xl flex justify-center items-center flex-shrink-0`}
            >
              <Icon name={styles.iconName} size={16} />
            </div>
          </div>
          <div className="flex-1 pb-4 inline-flex flex-col justify-start items-start gap-1">
            <div className="justify-start text-title text-large font-medium font-['Inter']">
              {title}
            </div>
            <div className="self-stretch inline-flex flex-col justify-start items-start">
              {mainMessage && (
                <div className="self-stretch justify-start text-content text-medium font-regular font-['Inter']">
                  {mainMessage}
                </div>
              )}
              {errors.length > 0 && (
                <ul className="self-stretch list-disc list-inside text-content text-medium font-regular font-['Inter'] space-y-1 ml-2">
                  {errors.map((error, index) => (
                    <li key={index}>{error}</li>
                  ))}
                </ul>
              )}
            </div>
          </div>
        </div>
        <div className="pt-1 flex justify-start items-center gap-2.5">
          <button
            onClick={(e) => {
              e.stopPropagation();
              e.preventDefault();
              onClose();
            }}
            aria-label="Fechar"
            className="w-7 h-7 flex items-center justify-center hover:opacity-70 transition-opacity flex-shrink-0"
          >
            <X className="w-7 h-7 text-utility-60" strokeWidth={1.4} />
          </button>
        </div>
      </div>
    );
  },
);

CustomToast.displayName = 'CustomToast';

export default CustomToast;
