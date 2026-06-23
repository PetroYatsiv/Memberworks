// Minimal typings for the Google Identity Services client we load in index.html.
interface GoogleCredentialResponse {
  credential: string;
}

interface GoogleIdConfig {
  client_id: string;
  callback: (response: GoogleCredentialResponse) => void;
}

interface GoogleButtonOptions {
  type?: 'standard' | 'icon';
  theme?: 'outline' | 'filled_blue' | 'filled_black';
  size?: 'small' | 'medium' | 'large';
  text?: string;
  shape?: string;
}

interface Window {
  google?: {
    accounts: {
      id: {
        initialize(config: GoogleIdConfig): void;
        renderButton(parent: HTMLElement, options: GoogleButtonOptions): void;
        prompt(): void;
      };
    };
  };
}
