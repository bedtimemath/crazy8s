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
defined( 'ABSPATH' ) || die();

/**
 * Class Plugin
 *
 * Main Plugin class
 *
 * @since 1.0.0
 */
class Widgets {

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
	public static function instance() {
		if ( is_null( self::$instance ) ) {
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
	private function include_widgets_files() {
		require_once 'widgets/class-coach-skus-dropdown.php';
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
	public function register_widgets() {
		// It's now safe to include Widgets files.
		$this->include_widgets_files();

		// Register the plugin widget classes.
		\Elementor\Plugin::instance()->widgets_manager->register_widget_type( new Widgets\CoachSkusDropdown() );
		\Elementor\Plugin::instance()->widgets_manager->register_widget_type( new Widgets\CoachOrdersList() );
	}

	/**
	 * Register Categories
	 *
	 * Register new Elementor categories.
	 *
	 * @since 1.0.0
	 * @access public
	 */
	function add_elementor_widget_categories( $elements_manager ) {

		$elements_manager->add_category(
			'crazy8s',
			[
				'title' => esc_html__( 'Crazy 8s', 'elementor-crazy8s-coaches' ),
				'icon' => 'fa fa-plug',
				'active' => true,
			]
		);
	}

	/**
	 *  Plugin class constructor
	 *
	 * Register plugin action hooks and filters
	 *
	 * @since 1.0.0
	 * @access public
	 */
	public function __construct() {
		// Register the widgets.
		add_action( 'elementor/widgets/widgets_registered', array( $this, 'register_widgets' ) );
		add_action( 'elementor/elements/categories_registered', array( $this, 'add_elementor_widget_categories' ) );
	}
}

// Instantiate the Widgets class.
Widgets::instance();
