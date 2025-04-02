<?php

/**
 * Widgets class.
 *
 * @category   Class
 * @package    ElementorCrazy8sCoaches
 * @subpackage WordPress
 * @author     Dug Steen <dug@softcrow.net>
 * @copyright  2025 Dug Steen
 * @license    https://opensource.org/licenses/GPL-3.0 GPL-3.0-only
 * @since      1.0.0
 * php version 8.1
 */

namespace ElementorCrazy8sCoaches;

// Security Note: Blocks direct access to the plugin PHP files.
defined('ABSPATH') || die();

/**
 * Class Plugin
 *
 * Main Plugin class
 *
 * @since 1.0.0
 */
class Widgets
{

	/**
	 * Instance
	 *
	 * @since 1.0.0
	 * @access private
	 * @static
	 *
	 * @var Plugin The single instance of the class.
	 */
	private static $instance = null;

	/**
	 * Instance
	 *
	 * Ensures only one instance of the class is loaded or can be loaded.
	 *
	 * @since 1.0.0
	 * @access public
	 *
	 * @return Plugin An instance of the class.
	 */
	public static function instance()
	{
		if (is_null(self::$instance)) {
			self::$instance = new self();
		}

		return self::$instance;
	}

	/**
	 * Include Widgets files
	 *
	 * Load widgets files
	 *
	 * @since 1.0.0
	 * @access private
	 */
	private function include_widgets_files()
	{
		require_once 'widgets/class-coach-kitpages-dropdown.php';
		require_once 'widgets/class-coach-orders-list.php';
	}

	/**
	 * Register Widgets
	 *
	 * Register new Elementor widgets.
	 *
	 * @since 1.0.0
	 * @access public
	 */
	public function register_widgets()
	{
		// It's now safe to include Widgets files.
		$this->include_widgets_files();

		// Register the plugin widget classes.
		\Elementor\Plugin::instance()->widgets_manager->register_widget_type(new Widgets\CoachKitPagesDropdown());
		\Elementor\Plugin::instance()->widgets_manager->register_widget_type(new Widgets\CoachOrdersList());
	}

	/**
	 * Register Categories
	 *
	 * Register new Elementor categories.
	 *
	 * @since 1.0.0
	 * @access public
	 */
	function add_elementor_widget_categories($elements_manager)
	{

		$elements_manager->add_category(
			'crazy8s',
			[
				'title' => esc_html__('Crazy 8s', 'elementor-crazy8s-coaches'),
				'icon' => 'fa fa-plug',
				'active' => true,
			]
		);
	}

	/**
	 * Register Settings Page
	 *
	 * Register the settings page
	 *
	 * @since 1.0.0
	 * @access public
	 */
	public function add_crazy8s_settings_page()
	{
		add_options_page(
			'Crazy 8s Widgets Settings',
			'Crazy 8s Widgets',
			'manage_options',
			'crazy8s-widgets-settings',
			'create_crazy8s_settings_page_html'
		);
	}

	/**
	 * Show Settings Page
	 *
	 * Output HTML for the settings page
	 *
	 * @since 1.0.0
	 * @access public
	 */
	function crazy8s_settings_page_html()
	{
		if (!current_user_can('manage_options')) {
			return;
		}
?>
		<div class="wrap">
			<h1><?php esc_html_e('Crazy 8s Widgets Settings', 'elementor-crazy8s-coaches'); ?></h1>
			<form action="options.php" method="post">
				<?php
				settings_fields('crazy8s_settings');
				do_settings_sections('crazy8s_settings');
				submit_button(__('Save Settings', 'elementor-crazy8s-coaches'));
				?>
			</form>
		</div>
	<?php
	}

	/**
	 * Crazy 8s Register Settings
	 *
	 * Register the Crazy 8s settings
	 *
	 * @since 1.0.0
	 * @access public
	 */
	function crazy8s_register_settings()
	{
		register_setting('crazy8s_settings', 'crazy8s_platform');
		register_setting('crazy8s_settings', 'crazy8s_api_key');

		add_settings_section(
			'crazy8s_settings_section',
			__('Settings', 'elementor-crazy8s-coaches'),
			null,
			'crazy8s_settings'
		);

		add_settings_field(
			'crazy8s_platform',
			__('Platform', 'elementor-crazy8s-coaches'),
			'crazy8s_platform_callback',
			'crazy8s_settings',
			'crazy8s_settings_section'
		);

		add_settings_field(
			'crazy8s_api_key',
			__('API Key', 'elementor-crazy8s-coaches'),
			'crazy8s_api_key_callback',
			'crazy8s_settings',
			'crazy8s_settings_section'
		);
	}

	function crazy8s_platform_callback()
	{
		$platform = get_option('crazy8s_platform');
	?>
		<select name="crazy8s_platform">
			<option value="development" <?php selected($platform, 'development'); ?>>Development</option>
			<option value="staging" <?php selected($platform, 'staging'); ?>>Staging</option>
			<option value="production" <?php selected($platform, 'production'); ?>>Production</option>
		</select>
	<?php
	}

	function crazy8s_api_key_callback()
	{
		$api_key = get_option('crazy8s_api_key');
	?>
		<input type="text" name="crazy8s_api_key" value="<?php echo esc_attr($api_key); ?>" />
<?php
	}

	/**
	 *  Plugin class constructor
	 *
	 * Register plugin action hooks and filters
	 *
	 * @since 1.0.0
	 * @access public
	 */
	public function __construct()
	{
		// Register the widgets.
		add_action('elementor/widgets/widgets_registered', array($this, 'register_widgets'));
		add_action('elementor/elements/categories_registered', array($this, 'add_elementor_widget_categories'));
		// Add the settings and settings page
		//add_action('admin_init', 'crazy8s_register_settings');
		add_action('admin_menu', 'add_crazy8s_settings_page');
	}
}

// Instantiate the Widgets class.
Widgets::instance();
